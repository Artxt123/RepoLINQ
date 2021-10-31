using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var samochody = WczytywaniePliku("paliwo.csv");

            //Sortujemy samochody po najoszczędniejszym spalaniu na autostradzie malejąco; następnie alfabetycznie po nazwie producenta
            var zapytanie = from samochod in samochody
                             where samochod.Producent == "Audi" && samochod.Rok == 2018
                             orderby samochod.SpalanieAutostrada descending, samochod.Producent ascending //ascending jest zawsze domyślnym sposobem sortowania i w sumie nie trzeba tego pisać
                            //Wybieramy tylko te dane, które potrzebujemy tworząc w tym celu nowy anonimowy obiekt:
                            select new
                             {
                                 samochod.Producent,
                                 samochod.Model,
                                 samochod.SpalanieAutostrada
                             };

            //SelectMany spłaszcza wszystkie kolekcje, które otrzymujemy w wyniku zapytania:
            //W tym przypadku noramlnie otrzymalibyśmy stringi z nazwą producenta, a string to kolekcja charów; dzięki SelectMany otzrymamy od razu same litery tzn. IEnumerable<char>
            var zapytanie2 = samochody.SelectMany(s => s.Producent).OrderBy(s => s);

            foreach (var litera in zapytanie2)
            {
                    Console.WriteLine(litera);
            }

            //foreach (var samochod in zapytanie.Take(10))
            //{
            //    Console.WriteLine($"{samochod.Producent} {samochod.Model} : {samochod.SpalanieAutostrada}");
            //}
        }


        private static List<Samochod> WczytywaniePliku(string sciezka)
        {
            return File.ReadAllLines(sciezka)
                       .Skip(1) //pomijamy pierwszą linię z pliku csv, która jest nagłówkiem
                       .Where(linia => linia.Length > 1) //filtrujemy i odrzucamy linie, które nie mają w sobie danych, (przy założeniu, że jak linia ma 1 lub 0 znaków, to ta linia nie ma danych)
                       .WSamochod() //Taki nasz select
                       .ToList();
        }

        /// <summary>
        /// STARE To samo co metoda WczytywaniePliku, tylko z wykorzytsaniem Query syntax
        /// </summary>
        /// <param name="sciezka">Scieżka do pliku</param>
        /// <returns></returns>
        //private static List<Samochod> WczytywaniePliku2(string sciezka)
        //{
        //    var zapytanie = from linia in File.ReadAllLines(sciezka).Skip(1)
        //                    where linia.Length > 1
        //                    select Samochod.ParsujCSV(linia);

        //    return zapytanie.ToList();
        //}
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
