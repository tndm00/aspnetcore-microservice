using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        private readonly OrderContext _orderContext;
        private readonly ILogger _logger;

        public OrderContextSeed(ILogger logger, OrderContext orderContext)
        {
            _logger = logger;
            _orderContext = orderContext;
        }

        public async Task InitializeAsync()
        {
            try
            {
                if (_orderContext.Database.IsSqlServer())
                {
                    await _orderContext.Database.MigrateAsync();
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "An error occurred while initalising the database");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAync();
                await _orderContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while initalising the database");
                throw;
            }
        }

        public async Task TrySeedAync() 
        {
            if (!_orderContext.Orders.Any())
            {
                await _orderContext.Orders.AddRangeAsync(
                    new Order
                    {
                        UserName = "customer1",
                        FirstName = "customer1",
                        LastName = "customer1",
                        EmailAddress = "customer1@gmail.com",
                        InvoiceAddress = "VietName",
                        ShippingAddress="LongHai",
                    });
            }
        }
    }
}
