using Microsoft.EntityFrameworkCore;

namespace Customer.API.Porsistence
{
    public static class CustomerContextSeed
    {
        public static IHost SeedCustomerData(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var customerContext = scope.ServiceProvider.GetService<CustomerContext>();
            customerContext.Database.MigrateAsync().GetAwaiter().GetResult();

            CreateCustomer(customerContext, "Customer1", "Customer1", "Customer1", "customer1@gmail.com").GetAwaiter().GetResult();
            CreateCustomer(customerContext, "Customer2", "Customer2", "Customer2", "customer2@gmail.com").GetAwaiter().GetResult();

            return host;
        }

        public static async Task CreateCustomer(CustomerContext customerContext,
            string userName, string firstName, string lastName, string emailAddress)
        {
            var customer = await customerContext.Customers.SingleOrDefaultAsync(x=>x.UserName.Equals(userName)
            || x.EmailAddress.Equals(emailAddress));

            if(customer == null)
            {
                var newCustomer = new Entities.Customer
                {
                    UserName = userName,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailAddress = emailAddress,
                };
                await customerContext.Customers.AddAsync(newCustomer);
                await customerContext.SaveChangesAsync();
            }
        }
    }
}
