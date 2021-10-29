using System;
using System.Collections.Generic;
using System.Linq;

namespace _3_Zapytania
{
    class Program
    {
        static void Main(string[] args)
        {
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

            var zapytanie = Enumerable.Empty<Film>();

            try
            {
                zapytanie = filmy.Filtr(f => f.Rok > 2002);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine(zapytanie.Count());

            var enumerator = zapytanie.GetEnumerator();

            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Tytul);
            }
        }
    }
}
