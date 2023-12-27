using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class TravelInfo
    {
        public TravelInfo()
        {
            Bookings = new HashSet<Booking>();
        }

        public int TravelInfoId { get; set; }
        public int? TouristSpotId { get; set; }
        public string? Name { get; set; }
        public double? Price { get; set; }
        public DateTime? StartingTime { get; set; }
        public DateTime? EndingTime { get; set; }
        public string? Description { get; set; }

        public virtual TouristSpot? TouristSpot { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
