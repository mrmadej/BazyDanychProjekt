using BazyDanychProjekt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BazyDanychProjekt.Data
{
    
    public class ApplicationDbContext : IdentityDbContext<Uzytkownik>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Hotel> Hotele { get; set; }
        public DbSet<Zdjecie> Zdjecia { get; set; }
        public DbSet<Opinia> Opinia{ get; set; }
        public DbSet<Pokoj> Pokoj{ get; set; }
        public DbSet<Rezerwacja> Rezerwacja{ get; set; }
        public DbSet<Uzytkownik> Uzytkownik{ get; set; }

    }
}
