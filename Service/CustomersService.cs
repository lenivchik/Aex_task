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

            string sqlExpression = @"SELECT c.NAME AS Customer, m.NAME AS Manager, SUM(o.AMOUNT) AS Amount
                                            FROM CUSTOMERS c INNER JOIN MANAGERS m ON c.MANAGERID = m.ID
                                            INNER JOIN ORDERS o ON c.ID = o.CUSTOMERID
                                            WHERE o.DATE >= @beginDate AND o.DATE <= @currentDate
                                            GROUP BY c.ID, c.NAME, m.NAME
                                            HAVING SUM(o.AMOUNT) > @sumAmount;";
        }



    }
}
