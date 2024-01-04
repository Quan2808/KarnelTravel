using System;
using System.Collections.Generic;

namespace WebApp.Models
{
    public partial class Feedback
    {
        public int FeedbackId { get; set; }
        public int? RatingId { get; set; }
        public string? Comment { get; set; }

        public virtual Rating? Rating { get; set; }
    }
}
