using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Value { get; set; }

        [MaxLength(int.MaxValue)]
        public string? Comment { get; set; }

        public string CustomerName { get; set; }

        public string CustomerPhone { get; set; }

        public DateTime CreatedAt { get; set; }

        public int BookingID { get; set; }

        [ForeignKey("BookingID")]
        public Booking? Booking { get; set; }

    }
}