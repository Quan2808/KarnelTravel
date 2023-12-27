using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Booking
    {
        public Booking()
        {
            Payments = new HashSet<Payment>();
        }

        public int BookingId { get; set; }
        public string UserId { get; set; } = null!;
        public int? TravelInfoId { get; set; }
        public DateTime? BookingTime { get; set; }
        public string? Status { get; set; }

        public virtual TravelInfo? TravelInfo { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
