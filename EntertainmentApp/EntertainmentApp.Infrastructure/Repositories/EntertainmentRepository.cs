using EntertainmentApp.Application.Services.Interfaces;
using EntertainmentApp.Domain.Entities;
using EntertainmentApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace EntertainmentApp.Infrastructure.Repositories
{
    public class EntertainmentRepository : IEntertainmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EntertainmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Entertainment> GetAllEntertainments()
        {
            return _context.Entertainments
                .Include(e => e.Genre)
                .Include(e => e.Creator)
                .ToList();
        }

        public Entertainment GetEntertainmentById(int id)
        {
            return _context.Entertainments
                .Include(e => e.Genre)
                .Include(e => e.Creator)
                .FirstOrDefault(e => e.Id == id);
        }

        public void AddEntertainment(Entertainment entertainment)
        {
            _context.Entertainments.Add(entertainment);
            _context.SaveChanges();
        }

        public void UpdateEntertainment(Entertainment entertainment)
        {
            _context.Entry(entertainment).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteEntertainment(int id)
        {
            var entertainment = _context.Entertainments.Find(id);
            if (entertainment != null)
            {
                entertainment.IsDeleted = true;
                _context.SaveChanges();
            }
        }
    }
}
