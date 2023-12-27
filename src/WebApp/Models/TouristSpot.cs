using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class TouristSpot
    {
        public TouristSpot()
        {
            TravelInfos = new HashSet<TravelInfo>();
        }

        public int TouristSpotId { get; set; }
        public int? HotelId { get; set; }
        public int? ResortId { get; set; }
        public int? RestaurantId { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual Resort? Resort { get; set; }
        public virtual Restaurant? Restaurant { get; set; }
        public virtual ICollection<TravelInfo> TravelInfos { get; set; }
    }
}
