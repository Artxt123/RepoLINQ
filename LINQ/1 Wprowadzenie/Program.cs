using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_Wprowadzenie
{
    class Program
    {
        static void Main(string[] args)
        {
            var sciezka = @"c:\windows";
            PokazDuzePlikiBezLinq(sciezka);
            
        }

        private static void PokazDuzePlikiBezLinq(string sciezka)
        {
            DirectoryInfo katalog = new DirectoryInfo(sciezka);
            FileInfo[] pliki = katalog.GetFiles();

            Array.Sort(pliki, new FileInfoComparer());

            //WYŚWIETLAMY 5 NAJWIĘKSZYCH PLIKÓW
            for (int i = 0; i < 5; i++)
            {
                var plik = pliki[i];
                Console.WriteLine($"{plik.Name, -20} : {plik.Length / 1024, 20:N0}KB");
            }

            //DO WYŚWIETLENIA WSZYSTKICH PLIKÓW
            //foreach (var plik in pliki)
            //{
            //    Console.WriteLine($"{plik.Name} : {plik.Length/1024}KB");
            //}
        }
    }
    public class FileInfoComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            //porównujemy wielkość pliku y z wielkością pliku x, aby ułożyć je od największego do najmniejszego; jak będzie na odwrót to wyświetlą się od najmniejszego do największego
            return y.Length.CompareTo(x.Length);
        }
    }
}
