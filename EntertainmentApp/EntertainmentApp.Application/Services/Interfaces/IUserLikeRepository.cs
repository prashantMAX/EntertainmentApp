using EntertainmentApp.Domain.Entities;

namespace EntertainmentApp.Application.Services.Interfaces
{
    public interface IUserLikeRepository
    {
        UserLike GetUserLikeByReviewAndUser(int reviewId, string userId);
        void AddUserLike(UserLike userLike);
        void UpdateUserLike(UserLike userLike);
        void DeleteUserLike(int userLikeId);
    }
}
