using System;
using System.Globalization;
namespace Samochody
{
    public class Samochod
    {
        public int Rok { get; set; }
        public string Producent { get; set; }
        public string  Model { get; set; }
        public double Pojemnosc { get; set; }
        public int IloscCylindrow { get; set; }
        public int SpalanieMiasto { get; set; }
        public int SpalanieAutostrada { get; set; }
        public int SpalanieMieszane { get; set; }

        internal static Samochod ParsujCSV(string linia)
        {
            var kolumny = linia.Split(','); //dzielimy każdą linię na kolumny; przy założeniu, że każda kolumna oddzielona jest od następnej przecinkiem

            //Potrzebuję tego, bo inaczej dane nie chcą przejść; u mnie w csv liczby double musiałby mieć przecinek, a tam są kropki
            double.TryParse(kolumny[3], NumberStyles.Any, CultureInfo.InvariantCulture, out double pojemnosc);

            return new Samochod
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
