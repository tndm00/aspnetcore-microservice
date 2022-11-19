
using Common.Logging;
using Contracts.Common.Interfaces;
using Customer.API.Controllers;
using Customer.API.Porsistence;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services;
using Customer.API.Services.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog(Serilogger.Configure);

Log.Information("Starting Customer Minimal API up");

try
{
    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
    builder.Services.AddDbContext<CustomerContext>(options => options.UseNpgsql(connectionString));

    //Config Services
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>()
        .AddScoped(typeof(ICustomerService), typeof(CustomerService));

    var app = builder.Build();

    app.MapGet("/", () => "Wellcome to Customer Minial API");

    app.MapGet("/api/customers", 
        async(ICustomerService customerService) => await customerService.GetCustomerAsync());

    app.MapCustomerAPI();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseHttpsRedirection();
    app.UseSwagger();

    app.UseSwaggerUI(x =>
    {
        x.SwaggerEndpoint("/swagger/v1/swagger.json",
            "Swagger Customer Minial API v1");
    });

    app.UseAuthorization();

    app.MapControllers();

    app.SeedCustomerData()
        .Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information("Shut down Customer API complete");
    Log.CloseAndFlush();
}
