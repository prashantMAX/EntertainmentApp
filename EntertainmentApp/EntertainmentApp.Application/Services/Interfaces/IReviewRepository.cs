using EntertainmentApp.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EntertainmentApp.Application.Services.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> GetReviewByIdAsync(int id);
        Task<IEnumerable<Review>> GetReviewsByEntertainmentIdAsync(int entertainmentId);
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(int reviewId);
    }
}
