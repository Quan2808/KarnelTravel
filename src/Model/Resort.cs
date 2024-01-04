using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Resort
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }

        public string? Location { get; set; }

        public string? Description { get; set; }

        public float? Price { get; set; }

        public string? Image { get; set; }

        public List<Rating>? Ratings { get; set; }
    }
}
