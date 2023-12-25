using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Booking
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public decimal? RatingValue { get; set; }
        public DateTime? BookingTime { get; set; }
        public string? Status { get; set; }
        public int? TravelId { get; set; }

        public virtual Travel? Travel { get; set; }
    }
}
