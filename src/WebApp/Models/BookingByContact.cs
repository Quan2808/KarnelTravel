using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class BookingByContact
    {
        public int Id { get; set; }
        public string ContactPhone { get; set; } = null!;
        public DateTime BookingTime { get; set; }
        public string Status { get; set; } = null!;
        public int ResortId { get; set; }
        public int HotelId { get; set; }
        public int RestaurantId { get; set; }

        public virtual Hotel Hotel { get; set; } = null!;
        public virtual Resort Resort { get; set; } = null!;
        public virtual Restaurant Restaurant { get; set; } = null!;
    }
}
