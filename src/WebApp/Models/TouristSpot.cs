using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class TouristSpot
    {
        public TouristSpot()
        {
            Travels = new HashSet<Travel>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Location { get; set; } = null!;
        public int ResortId { get; set; }
        public int HotelId { get; set; }
        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; } = null!;
        public virtual ICollection<Travel> Travels { get; set; }
    }
}
