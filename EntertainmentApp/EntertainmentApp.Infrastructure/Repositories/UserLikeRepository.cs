using EntertainmentApp.Application.Services.Interfaces;
using EntertainmentApp.Domain.Entities;
using EntertainmentApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EntertainmentApp.Infrastructure.Repositories
{
    public class UserLikeRepository : IUserLikeRepository
    {
        private readonly ApplicationDbContext _context;

        public UserLikeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public UserLike GetUserLikeByReviewAndUser(int reviewId, string userId)
        {
            return _context.UserLikes.FirstOrDefault(ul => ul.ReviewId == reviewId && ul.UserId == userId);
        }

        public void AddUserLike(UserLike userLike)
        {
            _context.UserLikes.Add(userLike);
            _context.SaveChanges();
        }

        public void UpdateUserLike(UserLike userLike)
        {
            _context.UserLikes.Update(userLike);
            _context.SaveChanges();
        }

        public void DeleteUserLike(int userLikeId)
        {
            var userLike = _context.UserLikes.Find(userLikeId);
            if (userLike != null)
            {
                userLike.IsDeleted = true;
                _context.UserLikes.Update(userLike);
                _context.SaveChanges();
            }
        }
    }
}
