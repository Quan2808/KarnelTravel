using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Restaurant
    {
        public Restaurant()
        {
            Ratings = new HashSet<Rating>();
            TouristSpots = new HashSet<TouristSpot>();
        }

        public int RestaurantId { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<TouristSpot> TouristSpots { get; set; }
    }
}
