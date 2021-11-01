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
                            join producent in producenci 
                                on new { samochod.Producent, samochod.Rok }
                                equals new { Producent = producent.Nazwa, producent.Rok } //aby equals działał to po obu stronach muszą być takie same nazwy; dlatego musieliśmy dodać słowo Producent przed producent.nazwa, bo producent.nazwa nie pasował do samochod.producent
                            orderby samochod.SpalanieAutostrada descending, samochod.Producent ascending //ascending jest zawsze domyślnym sposobem sortowania i w sumie nie trzeba tego pisać
                            //Wybieramy tylko te dane, które potrzebujemy tworząc w tym celu nowy anonimowy obiekt:
                            select new
                             {
                                 producent.Siedziba,
                                 samochod.Producent,
                                 samochod.Model,
                                 samochod.SpalanieAutostrada
                             };

            #region Gdybyśmy chcieli pobrać tylko 4 właściwości jak w zapytaniu 1.
            //var zapytanie3 = samochody.Join(producenci,
            //                                s => s.Producent,
            //                                p => p.Nazwa,
            //                                (s, p) => new
            //                                {
            //                                    p.Siedziba,
            //                                    s.Producent,
            //                                    s.Model,
            //                                    s.SpalanieAutostrada
            //                                })
            //                          .OrderByDescending(s => s.SpalanieAutostrada)
            //                          .ThenBy(s => s.Producent);
            #endregion
            #region Gdybyśmy chcieli pobrać wszystkie dane i potem dopiero wybrać 4 właściwości
            //var zapytanie2 = samochody.Join(producenci,
            //                                s => s.Producent,
            //                                p => p.Nazwa,
            //                                (s, p) => new                   //pobieramy wszystkie dane, cały samochod i cały producent do anonimowej zmiennej
            //                                {
            //                                    Samochod = s,
            //                                    Producent = p
            //                                })
            //                          .OrderByDescending(s => s.Samochod.SpalanieAutostrada)
            //                          .ThenBy(s => s.Samochod.Producent)
            //                          .Select(s => new
            //                          {
            //                              s.Producent.Siedziba,
            //                              s.Samochod.Producent,
            //                              s.Samochod.Model,
            //                              s.Samochod.SpalanieAutostrada
            //                          });
            #endregion

            var zapytanie2 = samochody.Join(producenci,
                                            s => new { s.Producent, s.Rok },
                                            p => new { Producent = p.Nazwa, p.Rok },
                                            (s, p) => new           //pobieramy wszystkie dane, cały samochod i cały producent do anonimowej zmiennej
                                            {
                                                Samochod = s,
                                                Producent = p
                                            })
                                      .OrderByDescending(s => s.Samochod.SpalanieAutostrada)
                                      .ThenBy(s => s.Samochod.Producent);
                                     
            //foreach (var samochod in zapytanie.Take(10))
            //{
            //    Console.WriteLine($"{samochod.Siedziba} - {samochod.Producent} {samochod.Model} : {samochod.SpalanieAutostrada}");
            //}
            foreach (var samochod in zapytanie2.Take(10))
            {
                Console.WriteLine($"{samochod.Producent.Siedziba} - {samochod.Samochod.Producent} {samochod.Samochod.Model} : {samochod.Samochod.SpalanieAutostrada}");
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
