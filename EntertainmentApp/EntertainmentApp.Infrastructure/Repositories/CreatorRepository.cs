using System.Collections.Generic;
using System.Linq;
using EntertainmentApp.Domain.Entities;
using EntertainmentApp.Application.Services.Interfaces;
using EntertainmentApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EntertainmentApp.Infrastructure.Repositories
{
    public class CreatorRepository : ICreatorRepository
    {
        private readonly ApplicationDbContext _context;

        public CreatorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Creator> GetAllCreators()
        {
            return _context.Creators
                .Where(c => !c.IsDeleted)
                .ToList();
        }

        public Creator GetCreatorById(int id)
        {
            return _context.Creators
                .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
        }

        public void AddCreator(Creator creator)
        {
            _context.Creators.Add(creator);
            _context.SaveChanges();
        }

        public void UpdateCreator(Creator creator)
        {
            _context.Entry(creator).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteCreator(int id)
        {
            var creator = _context.Creators.Find(id);
            if (creator != null)
            {
                creator.IsDeleted = true;
                _context.SaveChanges();
            }
        }
    }
}
