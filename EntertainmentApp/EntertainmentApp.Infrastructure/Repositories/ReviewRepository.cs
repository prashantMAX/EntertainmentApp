using EntertainmentApp.Application.Services.Interfaces;
using EntertainmentApp.Domain.Entities;
using EntertainmentApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntertainmentApp.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }

        public async Task<IEnumerable<Review>> GetReviewsByEntertainmentIdAsync(int entertainmentId)
        {
            return await _context.Reviews
                .Include(r => r.UserLikes)
                .Where(r => r.EntertainmentId == entertainmentId && !r.IsDeleted)
                .ToListAsync();
        }

        public async Task AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review != null)
            {
                review.IsDeleted = true;
                _context.Reviews.Update(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}
