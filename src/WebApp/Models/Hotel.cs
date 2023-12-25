using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Hotel
    {
        public Hotel()
        {
            BookingByContacts = new HashSet<BookingByContact>();
            Ratings = new HashSet<Rating>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string Location { get; set; } = null!;
        public string? Image { get; set; } = null!;

        public virtual ICollection<BookingByContact> BookingByContacts { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
