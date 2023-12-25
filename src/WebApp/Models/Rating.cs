using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Rating
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public decimal RatingValue { get; set; }
        public string Location { get; set; } = null!;
        public int ResortId { get; set; }
        public int HotelId { get; set; }
        public int RestaurantId { get; set; }
        public int TravelId { get; set; }

        public virtual Hotel Hotel { get; set; } = null!;
        public virtual Resort Resort { get; set; } = null!;
        public virtual Restaurant Restaurant { get; set; } = null!;
        public virtual Travel Travel { get; set; } = null!;
    }
}
