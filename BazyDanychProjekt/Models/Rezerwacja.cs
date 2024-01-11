using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BazyDanychProjekt.Models
{
    public class Rezerwacja
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Uzytkownik")]
        public string UzytkownikId { get; set; }
        public Uzytkownik Uzytkownik { get; set; }

        [ForeignKey("Pokoj")]
        public int PokojId { get; set; }
        public Pokoj Pokoj { get; set; }

        public DateTime DataPoczatek { get; set; }
        public DateTime DataKoniec { get; set; }
    }
}
