using Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories.Repository
{
    public interface ICustomerRepository
    {
        ICollection<Customer> GetCustomers();

        Customer GetCustomer(int customerId);

        //bool CustomerExists(string brand, string name);

        bool CustomerExists(int id);

        bool CreateCustomer(Customer customer);

        bool UpdateCustomer(Customer customer);

        bool DeleteCustomer(Customer customer);

        bool Save();
    }
}
