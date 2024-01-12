using System.ComponentModel.DataAnnotations;

namespace BazyDanychProjekt.Models
{
    public class DodajHotelViewModel
    {
        [Required(ErrorMessage = "Pole Nazwa jest wymagane.")]
        public string Nazwa { get; set; }

        [Required(ErrorMessage = "Pole Kraj jest wymagane.")]
        public string Kraj { get; set; }

        [Required(ErrorMessage = "Pole Miasto jest wymagane.")]
        public string Miasto { get; set; }

        [Required(ErrorMessage = "Pole Gwiazdki jest wymagane.")]
        [Range(1, 5, ErrorMessage = "Gwiazdki muszą być w zakresie od 1 do 5.")]
        public int Gwiazdki { get; set; }

        [Required(ErrorMessage = "Pole Opis jest wymagane.")]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Pole URL do zdjęcia jest wymagane.")]
        [Url(ErrorMessage = "Podaj poprawny URL do zdjęcia.")]
        public string url { get; set; }
    }
}
