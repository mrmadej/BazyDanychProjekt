using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BazyDanychProjekt.Models
{
    public class Pokoj
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Nazwa { get; set; }

        [MaxLength(50)]
        public string NumerPokoju { get; set; }

        [MaxLength(50)]
        public string TypPokoju { get; set; }

        public int LiczbaOsob { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Cena { get; set; }

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }

        public List<Rezerwacja> Rezerwacje { get; set; }
    }
}
