using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            BookingByContacts = new HashSet<BookingByContact>();
            Ratings = new HashSet<Rating>();
            TouristSpots = new HashSet<TouristSpot>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Location { get; set; } = null!;
        public string Image { get; set; } = null!;

        public virtual ICollection<BookingByContact> BookingByContacts { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<TouristSpot> TouristSpots { get; set; }
    }
}
