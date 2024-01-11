using System.ComponentModel.DataAnnotations;

namespace BazyDanychProjekt.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nazwa { get; set; }

        [MaxLength(255)]
        public string Kraj { get; set; }

        [MaxLength(255)]
        public string Miasto { get; set; }

        public int Gwiazdki { get; set; }

        [MaxLength(500)]
        public string Opis { get; set; }

        public List<Zdjecie> Zdjecia { get; set; }

        public List<Pokoj> Pokoje { get; set; }

        public List<Opinia> Opinie { get; set; }
        //public Hotel()
        //{
        //    Zdjecia = new List<Zdjecie>();
        //    // Inicjalizacja innych kolekcji, jeśli istnieją
        //}
    }
}
