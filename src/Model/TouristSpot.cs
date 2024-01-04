using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class TouristSpot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? HotelID { get; set; }

        public int? ResortID { get; set; }

        public int? RestaurantID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Location { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public string? Image { get; set; }

        [ForeignKey("HotelID")]
        public Hotel? Hotel { get; set; }

        [ForeignKey("ResortID")]
        public Resort? Resort { get; set; }

        [ForeignKey("RestaurantID")]
        public Restaurant? Restaurant { get; set; }
        public List<Rating>? Ratings { get; set; }
    }
}
