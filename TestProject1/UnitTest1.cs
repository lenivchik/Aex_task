using ConsoleApp1.Contexts;
using ConsoleApp1.Models;
using ConsoleApp1.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using Moq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text.Json;
using static System.Reflection.Metadata.BlobBuilder;

namespace TestProject1
{
    public class Tests
    {
        private DbContextOptions<ApplicationContext> options;
        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
                        
            using (var context = new ApplicationContext(options))
            {
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

        [Test]
        public void GetCustomers_Check_Test()
        {
            

            using (var context = new ApplicationContext(options))
            {
                var repository = new CustomerService(context);
                var result = repository.GetCustomers(DateTime.Now.AddMonths(-3), 1000);

                context.Database.EnsureDeleted();


                Assert.AreEqual(2, result.Count);
                Assert.IsTrue(result.Any(c => c.CustomerName == "Alex" && c.ManagerName == "Bogdan" && c.Amount == 1200));
                Assert.IsTrue(result.Any(c => c.CustomerName == "Anton" && c.ManagerName == "Ivan" && c.Amount == 1100));
            }


        }
    }
}


