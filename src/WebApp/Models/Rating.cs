using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Rating
    {
        public Rating()
        {
            Feedbacks = new HashSet<Feedback>();
        }

        public int RatingId { get; set; }
        public int? HotelId { get; set; }
        public int? ResortId { get; set; }
        public int? RestaurantId { get; set; }
        public string UserId { get; set; } = null!;
        public double? RatingValue { get; set; }

        public virtual Hotel? Hotel { get; set; }
        public virtual Resort? Resort { get; set; }
        public virtual Restaurant? Restaurant { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
