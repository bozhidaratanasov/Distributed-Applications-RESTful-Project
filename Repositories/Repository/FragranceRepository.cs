using Data.Context;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories.Repository
{
    public class FragranceRepository : IFragranceRepository
    {
        private readonly ApplicationDbContext _context;

        public FragranceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateFragrance(Fragrance fragrance)
        {
            _context.Fragrances.Add(fragrance);
            return Save();
        }

        public bool DeleteFragrance(Fragrance fragrance)
        {
            _context.Fragrances.Remove(fragrance);
            return Save();
        }

        public bool FragranceExists(string brand, string name)
        {
            return _context.Fragrances.Any(x => x.Brand.ToLower().Trim() == brand.ToLower().Trim() 
            && x.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool FragranceExists(int id)
        {
            return _context.Fragrances.Any(x => x.FragranceId == id);
        }

        public Fragrance GetFragrance(int fragranceId)
        {
            return _context.Fragrances.Where(x => x.FragranceId == fragranceId).FirstOrDefault();
        }

        public ICollection<Fragrance> GetFragrances()
        {
            return _context.Fragrances.ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateFragrance(Fragrance fragrance)
        {
            _context.Fragrances.Update(fragrance);
            return Save();
        }
    }
}
