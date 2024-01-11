using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BazyDanychProjekt.Models
{
    public class Uzytkownik
        : IdentityUser
    {
//        [Key]
//        public int Id { get; set; }

        [MaxLength(50)]
        public string Login { get; set; }
 
        [MaxLength(255)]
        public string Haslo { get; set; }

        [MaxLength(50)]
        public string Imie { get; set; }

        [MaxLength(50)]
        public string Nazwisko { get; set; }

        [MaxLength(50)]
        public string Rola { get; set; }

        public List<Opinia> Opinie { get; set; }
        public List<Rezerwacja> Rezerwacje { get; set; }

        public Uzytkownik() { }
    }
}
