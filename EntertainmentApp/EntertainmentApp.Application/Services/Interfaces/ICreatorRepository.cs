using System.Collections.Generic;
using EntertainmentApp.Domain.Entities;

namespace EntertainmentApp.Application.Services.Interfaces
{
    public interface ICreatorRepository
    {
        IEnumerable<Creator> GetAllCreators();
        Creator GetCreatorById(int id);
        void AddCreator(Creator creator);
        void UpdateCreator(Creator creator);
        void DeleteCreator(int id);
    }
}
