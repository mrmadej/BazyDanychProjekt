using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BazyDanychProjekt.Models
{
    public class Zdjecie
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Url { get; set; }

        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}
