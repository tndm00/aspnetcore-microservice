
using Product.API.Entities;
using ILogger = Serilog.ILogger;
namespace Product.API.Porsistence
{
    public class ProductContextSeed
    {
        public static async Task SeedProductAsyns(ProductContext productContext, ILogger logger)
        {
            if (!productContext.Products.Any())
            {
                productContext.AddRange(GetCatalogProducts());
                await productContext.SaveChangesAsync();
                logger.Information("Seeded data for Product DB associated with context {DbContextName}", nameof(ProductContext));
            }
        }

        private static IEnumerable<CatalogProduct> GetCatalogProducts()
        {
            return new List<CatalogProduct>()
            {
                new CatalogProduct()
                {
                    No = "No Fuck bitch1",
                    Name = "Name Fuck bitch1",
                    Summary = "Summary Fuck bitch1",
                    Description = "Description Fuck bitch1",
                    Price = (decimal) 177948.49
                },
                new CatalogProduct()
                {
                    No = "No bitch Fuck",
                    Name = "Name bitch Fuck",
                    Summary = "Summary bitch Fuck",
                    Description = "Description bitch Fuck",
                    Price = (decimal) 258963.79
                }
            };
        }
    }
}
