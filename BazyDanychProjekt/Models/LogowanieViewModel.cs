using System.ComponentModel.DataAnnotations;
namespace BazyDanychProjekt.Models
{
    public class LogowanieViewModel
    {
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Haslo { get; set; }

        [Display(Name = "Pamiętaj mnie")]
        public bool PamietajMnie { get; set; }
    }
}