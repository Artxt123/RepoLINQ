using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Samochody
{
    class Program
    {
        static void Main(string[] args)
        {
            #region OperacjeNaCultureInfo
            //Gdybym chciał wyświetlać w konsoli kropkę zamiast przecinka dla liczb np. double
            //var newCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            //newCulture.NumberFormat.NumberDecimalSeparator = ".";
            //CultureInfo.CurrentCulture = newCulture;
            #endregion

            //Ta linia kodu to zabezpieczenie; polega na tym, że jeżeli zostanie coś zmienione np. w klasie Samochod, to baza danych SamochodDB zostanie usunięte i zostanie utworzona nowa - pusta baza danych
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<SamochodDB>());
            WstawDane();
            ZapytanieDane();

        }

        private static void WstawDane()
        {
            var samochody = WczytywanieSamochodu("paliwo.csv");
            var db = new SamochodDB(); //dzięki tej linijce kodu i EntityFramework, zostanie stworzona baza danych o nazwie SamochodDB, w której zostanie utworzona tabela Samochody, która będzia miała kolumny takie jak właściwości w obiekcie Samochod
            db.Database.Log = Console.WriteLine;


            if (!db.Samochody.Any()) //jeżeli w bazie danych w tabeli samochody nie ma żadnego samochodu (tzn. jeżeli tabela jest pusta) to wtedy będziemy wstawiać samochody
            {
                foreach (var samochod in samochody)
                {
                    db.Samochody.Add(samochod);
                }
                db.SaveChanges();
            }
        }

        private static void ZapytanieDane()
        {
            var db = new SamochodDB();
            db.Database.Log = Console.WriteLine;

            var zapytanie = from samochod in db.Samochody
                            orderby samochod.SpalanieAutostrada descending, samochod.Producent ascending
                            select samochod;

            var zapytanie2 = db.Samochody.Where(s => s.Producent == "Audi")
                                         .OrderByDescending(s => s.SpalanieAutostrada)
                                         .ThenBy(s => s.Model)
                                         .Take(10);

            foreach (var samochod in zapytanie2)
            {
                Console.WriteLine($"{samochod.Producent} {samochod.Model} : {samochod.SpalanieAutostrada}");
            }
        }

        private static void ZapytanieXML()
        {
            //Jeżeli w danym XMLu są przestrzenie nazw to w zapytaniach również musimy się do nich odwoływać:
            XNamespace ns = "http://jakasstrona.pl/samochody/2018";
            XNamespace ex = "http://jakasstrona.pl/samochody/2018/ex";

            var dokument = XDocument.Load("paliwo.xml");

            var zapytanie = from samochod in dokument.Element(ns + "Samochody")?.Elements(ex + "Samochod") ?? Enumerable.Empty<XElement>() // ?? sprawi, że jak nie znajdzie Elementu "Samochody" to zostanie przekazany pusty Enumerable od XElement, aby Where, który jest dalej wywoływany nie zgłosił wyjątku; Pierwszy ? działałby jeżeli dalej nie byłoby where, wtedy po prostu nie znalezionoby dlszego elementu samochod
                            where samochod.Attribute("Producent")?.Value == "Ferrari" // ? sprawi, że jak nie będzie takiego atrybutu to zostanie przypisana wartość null; pomaga to gdy są atrybuty opcjonalne; wtedy dla tych elementów, które mają jakąś wartość pod tym atrybutem to zostanie ona zostanie zachowana; a tym co mają null lub w ogóle nie ma tego atrybutu to zostanie przypisane null i program nie zgłosi wyjątku
                            select new
                            {
                               Producent = samochod.Attribute("Producent").Value,
                               Model = samochod.Attribute("Model").Value
                            };

            var zapytanie2 = dokument.Descendants(ex + "Samochod") // krótsza bardziej niebezpieczna wersja, która mówi: dajcie mi którychkolwiek POTOMKÓW o nazwie "Samochod"
                                     .Where(s => s.Attribute("Producent").Value == "Ferrari")
                                     .Select(s =>
                                     {
                                         return new
                                         {
                                             Producent = s.Attribute("Producent").Value,
                                             Model = s.Attribute("Model").Value
                                         };
                                     });

            foreach (var samochod in zapytanie2)
            {
                Console.WriteLine($"{samochod.Producent} {samochod.Model}");
            }
        }

        private static void TworzenieXML()
        {
            XNamespace ns = "http://jakasstrona.pl/samochody/2018";
            XNamespace ex = "http://jakasstrona.pl/samochody/2018/ex";

            var rekordy = WczytywanieSamochodu("paliwo.csv");

            var dokument = new XDocument();
            var samochody = new XElement(ns + "Samochody", from rekord in rekordy
                                                      select new XElement(ex + "Samochod",
                                                                       new XAttribute("Rok", rekord.Rok),
                                                                       new XAttribute("Producent", rekord.Producent),
                                                                       new XAttribute("Model", rekord.Model),
                                                                       new XAttribute("SpalanieAutostrada", rekord.SpalanieAutostrada),
                                                                       new XAttribute("SpalanieMiasto", rekord.SpalanieMiasto),
                                                                       new XAttribute("SpalanieMieszane", rekord.SpalanieMieszane)));

            //Jako nowy atrybut do Samochody dodajemy przestrzeń nazw xmlns:ex o wartości kryjącej się pod zmienną ex
            //Dzięki tej linijce kodu, dalsze elementy, które będą miały przestrzeń nazw ex, będą odwoływać się do niej za pomocą prefiksu ex (nie będą musiały w sobie zawierać długiej nazwy tej przestrzeni nazw), który będzie aliasem do xmlns:ex zawartej jako atrybut Samochody
            samochody.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));

            dokument.Add(samochody);
            dokument.Save("paliwo.xml");
        }

        private static List<Producent> WczytywanieProducenci(string sciezka)
        {
            var zapytanie = File.ReadAllLines(sciezka)
                                .Skip(1)
                                .Where(linia => linia.Length > 1)
                                .Select(l =>
                                {
                                    var kolumny = l.Split(',');
                                    return new Producent
                                    {
                                        Nazwa = kolumny[0],
                                        Siedziba = kolumny[1],
                                        Rok = int.Parse(kolumny[2]),
                                    };
                                });
            return zapytanie.ToList();
        }

        private static List<Samochod> WczytywanieSamochodu(string sciezka)
        {
            return File.ReadAllLines(sciezka)
                       .Skip(1) //pomijamy pierwszą linię z pliku csv, która jest nagłówkiem
                       .Where(linia => linia.Length > 1) //filtrujemy i odrzucamy linie, które nie mają w sobie danych, (przy założeniu, że jak linia ma 1 lub 0 znaków, to ta linia nie ma danych)
                       .WSamochod() //Taki nasz select
                       .ToList();
        }
    }

    public static class SamochodRozszerzenie
    {
        /// <summary>
        /// Taki nasz własny select, który od razu przekształca przefiltorowane dane w obiekty typu: Samochod
        /// </summary>
        /// <param name="zrodlo"></param>
        /// <returns></returns>
        public static IEnumerable<Samochod> WSamochod(this IEnumerable<string> zrodlo)
        {
            foreach (var linia in zrodlo)
            {
                var kolumny = linia.Split(','); //dzielimy każdą linię na kolumny; przy założeniu, że każda kolumna oddzielona jest od następnej przecinkiem

                //Potrzebuję tego, bo inaczej dane nie chcą przejść; u mnie w csv liczby, które chcę zapisać jako double musiałby mieć przecinek, a tam są kropki
                double.TryParse(kolumny[3], NumberStyles.Any, CultureInfo.InvariantCulture, out double pojemnosc);

                yield return new Samochod
                {
                    Rok = int.Parse(kolumny[0]),
                    Producent = kolumny[1],
                    Model = kolumny[2],
                    Pojemnosc = pojemnosc,
                    IloscCylindrow = int.Parse(kolumny[4]),
                    SpalanieMiasto = int.Parse(kolumny[5]),
                    SpalanieAutostrada = int.Parse(kolumny[6]),
                    SpalanieMieszane = int.Parse(kolumny[7])
                };
            }

        }
    }
}
