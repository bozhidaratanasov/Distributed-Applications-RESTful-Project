using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Repository
{
    public interface ISaleRepository
    {
        ICollection<Sale> GetSales();

        Sale GetSale(int saleId);

        bool SaleExists(int id);

        bool CreateSale(Sale sale);

        bool UpdateSale(Sale sale);

        bool DeleteSale(Sale sale);

        bool Save();
    }
}
