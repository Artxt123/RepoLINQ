using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_Funkcje
{
    class Program
    {
        static void Main(string[] args)
        {
            //Func zawsze coś zwraca, określamy to jako ostatni parametr
            Func<int, int> potegowanie = x => x * x;

            //Najkrótsze wyrażenie lambda
            //Func<int, int, int> dodawanie = (a, b) => a + b;
            //Wyrażenie lambda można też bardziej rozbudować:
            Func<int, int, int> dodawanie = (int a, int b) =>
            {
                var temp = a + b;
                return temp;
            };

            //Action nic nie zwraca (tzn. zwraca void)
            Action<int> wypisz = x => Console.WriteLine(x);

            wypisz(potegowanie(dodawanie(9, 1)));

            var programisci = new Pracownik[]
            {
                new Pracownik{Id = 1, Imie = "Marek", Nazwisko = "Rajkiewicz"},
                new Pracownik{Id = 2, Imie = "Wojciech", Nazwisko = "Wiertlewski"},
                new Pracownik{Id = 3, Imie = "Piotr", Nazwisko = "Lis"},
                new Pracownik{Id = 4, Imie = "Wojciech", Nazwisko = "Karbowiak"}
            };

            var kierowcy = new List<Pracownik>()
            {
                new Pracownik{Id = 5, Imie = "Michał", Nazwisko = "Andrzejak"},
                new Pracownik{Id = 6, Imie = "Marcin", Nazwisko = "Poradzisz"},
                new Pracownik{Id = 7, Imie = "Tomasz", Nazwisko = "Nowak"}
            };

            //Wypisujemy programistów, których imię ma 5 liter, posortowani po imionach w kolejności alfabeycznej od końca
            var zapytanie = programisci.Where(p => p.Imie.Length == 5)
                                       .OrderByDescending(p => p.Imie)
                                       .Select(p => p); //Select w Method Syntax jest opcjonalny, nie trzeba tego pisać.

            var zapytanie2 = from programista in programisci
                                 where programista.Imie.Length == 5
                                 orderby programista.Imie descending
                                 select programista;

            var iloscZwróconychZapytan = zapytanie2.Count();

            foreach (var osoba in zapytanie2)
            {
                Console.WriteLine($"{osoba.Imie}");
            }
            Console.WriteLine(iloscZwróconychZapytan);
        }

        //1. Metoda nazwana: przekazujemy jako: Where(RozpoczynaNaM)
        private static bool RozpoczynaNaM(Pracownik pracownik)
        {
            return pracownik.Imie.StartsWith("M");
        }
    }
}
