using System.Data.Entity;

namespace Samochody
{
    public class SamochodDB : DbContext
    {
        public DbSet<Samochod> Samochody { get; set; }
    }
}
