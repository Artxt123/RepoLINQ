using System;
using System.Collections.Generic;
using System.Linq;

namespace _3_Zapytania
{
    class Program
    {
        static void Main(string[] args)
        {
            //Używając operatorów strumieniowych możemy działać na praktycznie nieskończonych kolekcjach (tak jak ta poniżej, ponieważ w nieskończoność generuje liczby)
            var liczby = NaszLinq.LiczbyLosowe().Where(l => l > 0.5).Take(10);

            foreach (var liczba in liczby)
            {
                Console.WriteLine(liczba);
            }

            var filmy = new List<Film>
            {
                new Film{Tytul = "Siedem", Gatunek = "Thriller", Ocena = 8.3f, Rok = 1995},
                new Film{Tytul = "Efekt motyla", Gatunek = "Thriller", Ocena = 7.8f, Rok = 2004},
                new Film{Tytul = "127 godzin", Gatunek = "Dramat", Ocena = 7.1f, Rok = 2010},
                new Film{Tytul = "Skazani na Shawshank", Gatunek = "Dramat", Ocena = 8.7f, Rok = 1994},
                new Film{Tytul = "Zielona mila", Gatunek = "Dramat", Ocena = 8.6f, Rok = 1999},
                new Film{Tytul = "Forrest Gump", Gatunek = "Komedia", Ocena = 8.5f, Rok = 1994},
                new Film{Tytul = "Piękny umysł", Gatunek = "Dramat", Ocena = 8.3f, Rok = 2001},
                new Film{Tytul = "Gladiator", Gatunek = "Dramat", Ocena = 8.1f, Rok = 2000}
            };

            //Where jest typu strumieniowego, OrderBy nie jest.
            //Tabelka co jest wykonywane natychamistowo; odroczone i strumieniowo; odroczone i niestrumieniowo:
            //https://docs.microsoft.com/pl-pl/dotnet/csharp/programming-guide/concepts/linq/classification-of-standard-query-operators-by-manner-of-execution

            var zapytanie = from film in filmy
                            where film.Rok > 2002
                            orderby film.Ocena descending
                            select film;

            var enumerator = zapytanie.GetEnumerator();

            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Tytul);
            }
        }
    }
}
