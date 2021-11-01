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

            var samochody = WczytywanieSamochodu("paliwo.csv");
            var producenci = WczytywanieProducenci("producent.csv");

            var zapytanie = from samochod in samochody
                            join producent in producenci on samochod.Producent equals producent.Nazwa
                            orderby samochod.SpalanieAutostrada descending, samochod.Producent ascending //ascending jest zawsze domyślnym sposobem sortowania i w sumie nie trzeba tego pisać
                            //Wybieramy tylko te dane, które potrzebujemy tworząc w tym celu nowy anonimowy obiekt:
                            select new
                             {
                                 producent.Siedziba,
                                 samochod.Producent,
                                 samochod.Model,
                                 samochod.SpalanieAutostrada
                             };

            foreach (var samochod in zapytanie.Take(10))
            {
                Console.WriteLine($"{samochod.Siedziba} - {samochod.Producent} {samochod.Model} : {samochod.SpalanieAutostrada}");
            }
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
