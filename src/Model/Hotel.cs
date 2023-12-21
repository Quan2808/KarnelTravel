using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Location { get; set; }
        [StringLength(255)]
        public int? Price { get; set; }
        public string Description { get; set; }
        public string? Image {  get; set; }
    }
}
