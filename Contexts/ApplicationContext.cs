using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;


namespace ConsoleApp1.Contexts
{
    public class ApplicationContext : DbContext
    {
        private string connectionString;


        public ApplicationContext(string connectionString) => this.connectionString = connectionString;


        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Manager> Managers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
