using EntertainmentApp.Domain.Entities;
using System.Collections.Generic;

namespace EntertainmentApp.Application.Services.Interfaces
{
    public interface IEntertainmentRepository
    {
        IEnumerable<Entertainment> GetAllEntertainments();
        Entertainment GetEntertainmentById(int entertainmentId);
        void AddEntertainment(Entertainment entertainment);
        void UpdateEntertainment(Entertainment entertainment);
        void DeleteEntertainment(int entertainmentId);
    }
}
