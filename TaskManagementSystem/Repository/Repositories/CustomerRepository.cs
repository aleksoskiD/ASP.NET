using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Repository.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private Database db = new Database();

        public bool Create(Customer customer)
        {
            if(customer != null)
            {
                customer.DateCreated = DateTime.Now;
                db.Customers.Add(customer);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            var dbCustomer = GetById(id);
            var newCustomer = dbCustomer;
            if(dbCustomer != null)
            {
                newCustomer.IsActive = false;
                db.Entry(dbCustomer).CurrentValues.SetValues(newCustomer);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Customer> GetAll()
        {
            return db.Customers.ToList();
        }

        public Customer GetById(int id)
        {
            return db.Customers.FirstOrDefault(z => z.ID == id);
        }

        public bool Update(Customer customer)
        {
            var dbCustomer = GetById(customer.ID);
            if(dbCustomer != null)
            {
                db.Entry(dbCustomer).CurrentValues.SetValues(customer);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
