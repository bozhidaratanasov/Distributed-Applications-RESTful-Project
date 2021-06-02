using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories.Repository
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDbContext _context;

        public SaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateSale(Sale sale)
        {
            _context.Sales.Add(sale);
            return Save();
        }

        public bool DeleteSale(Sale sale)
        {
            _context.Sales.Remove(sale);
            return Save();
        }

        public bool SaleExists(int id)
        {
            return _context.Sales.Any(x => x.SaleId == id);
        }

        public Sale GetSale(int saleId)
        {
            return _context.Sales.Where(x => x.SaleId == saleId)
                .Include(x=>x.Fragrance)
                .Include(x=>x.Customer)
                .FirstOrDefault();
        }

        public ICollection<Sale> GetSales()
        {
            return _context.Sales.Include(x => x.Fragrance).Include(x=>x.Customer).ToList();
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateSale(Sale sale)
        {
            _context.Sales.Update(sale);
            return Save();
        }
    }
}
