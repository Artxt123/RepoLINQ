using System;
using System.Collections.Generic;
//using System.Linq;
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
            //Dzięki temu, że tablica i lista mogą korzystać z IEnumerable<T>, możemy iterować po tych kolekcjach
            foreach (var osoba in programisci)
            {
                Console.WriteLine($"{osoba.Imie} {osoba.Nazwisko}");
            }

            //Teraz zrobimy to samo co w pętli foreach, tylko krok po kroku:

            IEnumerator<Pracownik> enumerator = kierowcy.GetEnumerator();

            while (enumerator.MoveNext()) //MoveNext() zwraca true, gdy można przejść do nast. elementu; false gdy nie można
            {
                Console.WriteLine($"{enumerator.Current.Imie} {enumerator.Current.Nazwisko}");
            }
        }
    }
}
