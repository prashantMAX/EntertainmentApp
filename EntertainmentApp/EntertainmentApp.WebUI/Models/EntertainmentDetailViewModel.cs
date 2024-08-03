using EntertainmentApp.Domain.Entities;
using EntertainmentApp.Infrastructure.Identity;
using System.Collections.Generic;

namespace EntertainmentApp.WebUI.Models
{
    public class EntertainmentDetailViewModel
    {
        public Entertainment? Entertainment { get; set; }
        public IEnumerable<Review>? Reviews { get; set; }
        //public int UserLikesCount { get; set; }

        public Review? NewReview { get; set; } // New property for creating a review

        public string? UserName { get; set; }
        public double? AverageRating { get; set; }

    }
}
