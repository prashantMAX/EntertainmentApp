using System.Collections.Generic;
using EntertainmentApp.Domain.Entities;

namespace EntertainmentApp.Application.Services.Interfaces
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetAllGenres();
        Genre GetGenreById(int id);
        void AddGenre(Genre genre);
        void UpdateGenre(Genre genre);
        void DeleteGenre(int id);
    }
}
