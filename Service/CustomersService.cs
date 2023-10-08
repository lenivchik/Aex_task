using ConsoleApp1.Contexts;
using ConsoleApp1.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Service
{
    public class CustomerService
    {
        private ApplicationContext context;
        private string connectionString;

        public CustomerService(ApplicationContext context)
        {
            this.context = context;
        }

        public  List<CustomerViewModel> GetCustomers(DateTime beginDate, decimal sumAmount)
        {
            DateTime currentDate = DateTime.Now;
            var customers = context.Customers
                .Where(c => c.Orders.Any(o => o.Date >= beginDate && o.Date <= currentDate))
                .Where(c => c.Orders.Sum(o => o.Amount) > sumAmount)
                .Select(c => new CustomerViewModel
                {
                    CustomerName = c.Name,
                    ManagerName = c.Manager.Name,
                    Amount = c.Orders.Sum(o => o.Amount)
                })
                .ToList();

            return customers;

        }

    }
}
