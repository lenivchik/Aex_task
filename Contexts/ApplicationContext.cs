using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;


namespace ConsoleApp1.Contexts
{
    public class ApplicationContext : DbContext
    {
        public  DbSet<Customer> Customers { get; set; }
        public  DbSet<Manager> Managers { get; set; }
        public   DbSet<Order> Orders { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

    }


}
