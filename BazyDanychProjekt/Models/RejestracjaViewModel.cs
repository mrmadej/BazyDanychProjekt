using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
namespace BazyDanychProjekt.Models
{
    public class RejestracjaViewModel
    {
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} i co najwyżej {1} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Haslo { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Haslo", ErrorMessage = "Hasło i potwierdzenie hasła nie pasują do siebie.")]
        public string PotwierdzHaslo { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [Display(Name = "Imię")]
        public string Imie { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [Display(Name = "Nazwisko")]
        public string Nazwisko { get; set; }
    }
}