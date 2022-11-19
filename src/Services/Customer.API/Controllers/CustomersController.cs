using Customer.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers
{
    public static class CustomersController
    {
        public static void MapCustomerAPI(this WebApplication app)
        {
            app.MapGet("/api/customers/{userName}",
            async (string userName, ICustomerService customerService) =>
            {
                var customer = await customerService.GetCustomerByUserName(userName);
                return customer != null ? customer : Results.NotFound();
            });
        }
    }
}
