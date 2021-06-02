using Data.Context;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
            private readonly ApplicationDbContext _context;

            public CustomerRepository(ApplicationDbContext context)
            {
                _context = context;
            }

            public bool CreateCustomer(Customer customer)
            {
                _context.Customers.Add(customer);
                return Save();
            }

            public bool DeleteCustomer(Customer customer)
            {
                _context.Customers.Remove(customer);
                return Save();
            }

            public bool CustomerExists(int id)
            {
                return _context.Customers.Any(x => x.CustomerId == id);
            }

            public Customer GetCustomer(int customerId)
            {
                return _context.Customers.Where(x => x.CustomerId == customerId).FirstOrDefault();
            }

            public ICollection<Customer> GetCustomers()
            {
                return _context.Customers.ToList();
            }

            public bool Save()
            {
                return _context.SaveChanges() >= 0 ? true : false;
            }

            public bool UpdateCustomer(Customer customer)
            {
                _context.Customers.Update(customer);
                return Save();
            }
        }
}
