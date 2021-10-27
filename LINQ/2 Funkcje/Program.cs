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
            IEnumerable<Pracownik> programisci = new Pracownik[]
            {
                new Pracownik{Id = 1, Imie = "Marek", Nazwisko = "Rajkiewicz"},
                new Pracownik{Id = 2, Imie = "Wojciech", Nazwisko = "Wiertlewski"},
                new Pracownik{Id = 3, Imie = "Piotr", Nazwisko = "Lis"},
                new Pracownik{Id = 4, Imie = "Wojciech", Nazwisko = "Karbowiak"}
            };

            IEnumerable<Pracownik> kierowcy = new List<Pracownik>()
            {
                new Pracownik{Id = 5, Imie = "Michał", Nazwisko = "Andrzejak"},
                new Pracownik{Id = 6, Imie = "Marcin", Nazwisko = "Poradzisz"},
                new Pracownik{Id = 7, Imie = "Tomasz", Nazwisko = "Nowak"}
            };

            //Do Where możemy podać metodę na 3 różne sposoby: 1) Metoda nazwana; 2) Metoda anonimowa (metoda inline); 3) Wyrażenie lambda
            //1:Użycie w Where metody nazwanej
            foreach (var osoba in programisci.Where(RozpoczynaNaM))
            {
                Console.WriteLine($"{osoba.Imie}");
            }

            //2:Metoda anonimowa (tzw. Metoda inline) to ta sama co metoda nazwana, tylko bez początku: public static bool Nazwa....; zamiast tego należy dodać słowo kluczowe delegate
            foreach (var osoba in programisci.Where(delegate (Pracownik pracownik)
                                                   { return pracownik.Imie.StartsWith("M"); }))
            {
                Console.WriteLine($"{osoba.Imie}");
            }

            //3: Wyrażenie lambda:
            foreach (var osoba in programisci.Where(p => p.Imie.StartsWith("M")))
            {
                Console.WriteLine($"{osoba.Imie}");
            }
        }

        //1. Metoda nazwana: przekazujemy jako: Where(RozpoczynaNaM)
        private static bool RozpoczynaNaM(Pracownik pracownik)
        {
            return pracownik.Imie.StartsWith("M");
        }
    }
}
