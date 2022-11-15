using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Customer.API.Porsistence
{
    public class CustomerContext : DbContext
    {
        public CustomerContext()
        {

        }

        public CustomerContext(DbContextOptions<CustomerContext> options) :base(options)
        {
        }

        public DbSet<Entities.Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Entities.Customer>().HasIndex(x => x.UserName).IsUnique();
            modelBuilder.Entity<Entities.Customer>().HasIndex(x => x.EmailAddress).IsUnique();
        }
    }
}
