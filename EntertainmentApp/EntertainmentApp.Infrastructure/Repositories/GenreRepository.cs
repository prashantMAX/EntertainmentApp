using System.Collections.Generic;
using System.Linq;
using EntertainmentApp.Domain.Entities;
using EntertainmentApp.Application.Services.Interfaces;
using EntertainmentApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EntertainmentApp.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Genre> GetAllGenres()
        {
            return _context.Genres
                .Where(g => !g.IsDeleted)
                .ToList();
        }

        public Genre GetGenreById(int id)
        {
            return _context.Genres
                .FirstOrDefault(g => g.Id == id && !g.IsDeleted);
        }

        public void AddGenre(Genre genre)
        {
            _context.Genres.Add(genre);
            _context.SaveChanges();
        }

        public void UpdateGenre(Genre genre)
        {
            _context.Entry(genre).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteGenre(int id)
        {
            var genre = _context.Genres.Find(id);
            if (genre != null)
            {
                genre.IsDeleted = true;
                _context.SaveChanges();
            }
        }
    }
}
