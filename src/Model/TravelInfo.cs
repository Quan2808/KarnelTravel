using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class TravelInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int TouristSpotID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public float Price { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        [ForeignKey("TouristSpotID")]
        public TouristSpot? TouristSpot { get; set; }

        public List<Rating>? Ratings { get; set; }
    }
}
