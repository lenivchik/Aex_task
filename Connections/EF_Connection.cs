using ConsoleApp1.Contexts;
using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Connections
{
    public class EF_Connection
    {
        private string connectionString;
        public EF_Connection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddData()
        {
            try
            {
                using (var context = new ApplicationContext(connectionString))
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                    Manager manager1 = new Manager { Name = "Ivan" };
                    Manager manager2 = new Manager { Name = "Bogdan" };
                    Customer customer1 = new Customer { Name = "Anton", Manager = manager1 };
                    Customer customer2 = new Customer { Name = "Alex", Manager = manager2 };
                    Customer customer3 = new Customer { Name = "Max", Manager = manager2 };
                    Order order1 = new Order { Amount = 1200, Customer = customer2, Date = DateTime.Now };
                    Order order2 = new Order { Amount = 500, Customer = customer1, Date = DateTime.Now };
                    Order order3 = new Order { Amount = 600, Customer = customer1, Date = DateTime.Now };
                    Order order4 = new Order { Amount = 500, Customer = customer3, Date = DateTime.Now };
                    context.Managers.AddRange(manager1, manager2);
                    context.Customers.AddRange(customer1, customer2, customer3);
                    context.Orders.AddRange(order1, order2, order3, order4);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void WriteList(List<CustomerViewModel> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine($"{item.CustomerName} {item.ManagerName} {item.Amount}");
            }

        }

        //public void ReadData()
        //{
        //    using (var context = new ApplicationContext(connectionString))
        //    {
        //        // получаем объекты из бд и выводим на консоль
        //        var users = context.Users.ToList();
        //        Console.WriteLine("Users list:");
        //        foreach (User u in users)
        //        {
        //            Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
        //        }
        //    }
        //}

        public List<CustomerViewModel> GetCustomers(DateTime beginDate, decimal sumAmount)
        {
            DateTime currentDate = DateTime.Now;

            using (var context = new ApplicationContext(connectionString)) 
            {
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
                WriteList(customers);
                return customers;
            }
        }
    }
}
