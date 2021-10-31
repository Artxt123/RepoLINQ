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

            var samochody = WczytywaniePliku2("paliwo.csv");

            //Sortujemy samochody po najoszczędniejszym spalaniu na autostradzie malejąco; następnie alfabetycznie po nazwie producenta
            var zapytanie = samochody.OrderByDescending(s => s.SpalanieAutostrada)
                                     .ThenBy(s => s.Producent)
                                     .Select(s => s)
                                     .FirstOrDefault(s => s.Producent == "ccc" && s.Rok == 2018);

            var zapytanie2 = from samochod in samochody
                             where samochod.Producent == "Audi" && samochod.Rok == 2018
                             orderby samochod.SpalanieAutostrada descending, samochod.Producent ascending
                             select samochod;

            if (zapytanie != null)
            {
                Console.WriteLine($"{zapytanie.Producent} {zapytanie.Model} : {zapytanie.SpalanieAutostrada}");
            }

            foreach (var samochod in zapytanie2.Take(10))
            {
                Console.WriteLine($"{samochod.Producent} {samochod.Model} : {samochod.SpalanieAutostrada}");
            }
        }

        /// <summary>
        /// To samo co metoda WczytywaniePliku, tylko z wykorzytsaniem Query syntax
        /// </summary>
        /// <param name="sciezka">Scieżka do pliku</param>
        /// <returns></returns>
        private static List<Samochod> WczytywaniePliku2(string sciezka)
        {
            var zapytanie = from linia in File.ReadAllLines(sciezka).Skip(1)
                            where linia.Length > 1
                            select Samochod.ParsujCSV(linia);

            return zapytanie.ToList();
        }

        private static List<Samochod> WczytywaniePliku(string sciezka)
        {
            return File.ReadAllLines(sciezka)
                       .Skip(1) //pomijamy pierwszą linię z pliku csv, która jest nagłówkiem
                       .Where(linia => linia.Length > 1) //filtrujemy i odrzucamy linie, które nie mają w sobie danych, (przy założeniu, że jak linia ma 1 lub 0 znaków, to ta linia nie ma danych)
                       .Select(Samochod.ParsujCSV) //przekształcamy pobrane linie na obiekty typu Samochod, dzięki metodzie ParsujCSV
                       .ToList(); //Zamieniamy pobrane dane na listę samochodów
        }
    }
}
