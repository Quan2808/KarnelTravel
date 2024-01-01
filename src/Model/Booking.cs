using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace Model
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? TravelInfoID { get; set; }

        public int? TouristSpotID { get; set; }

        public int? HotelID { get; set; }

        public int? ResortID { get; set; }

        public int? RestaurantID { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string CustomerPhone { get; set; }

        [Required]
        public DateTime CheckIn { get; set; }

        [Required]
        public DateTime CheckOut { get; set; }

        public float TotalPrice { get; set; }

        public BookingStatus Status { get; set; }

        [ForeignKey("TouristSpotID")]
        public TouristSpot? TouristSpot { get; set; }

        [ForeignKey("HotelID")]
        public Hotel? Hotel { get; set; }

        [ForeignKey("ResortID")]
        public Resort? Resort { get; set; }

        [ForeignKey("RestaurantID")]
        public Restaurant? Restaurant { get; set; }

        [ForeignKey("TravelInfoID")]
        public TravelInfo? TravelInfo { get; set; }

    }

    public enum BookingStatus
    {
        Processing,
        Confirmed,
        Cancelled,
        Completed,
    }
}