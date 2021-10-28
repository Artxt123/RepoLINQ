using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3_Zapytania
{
    public static class NaszLinq
    {
        public static IEnumerable<T> Filtr<T>(this IEnumerable<T> zrodlo, Func<T, bool> predicate)
        {
            var wynik = new List<T>();

            foreach (var item in zrodlo)
            {
                if (predicate(item))
                {
                    wynik.Add(item);
                }
            }

            return wynik;
        }
    }
}
