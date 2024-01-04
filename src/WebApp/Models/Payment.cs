using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int? BookingId { get; set; }
        public string? Type { get; set; }

        public virtual Booking? Booking { get; set; }
    }
}
