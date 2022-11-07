using Contracts.Common.Interfaces;
using Product.API.Entities;
using Product.API.Porsistence;

namespace Product.API.Repositories.Interfaces
{
    public interface IProductRepository : IRepsoitoryBaseAsync<CatalogProduct, long, ProductContext>
    {
        Task<IEnumerable<CatalogProduct>> GetProducts();
        Task<CatalogProduct> GetProductById(long id);
        Task<CatalogProduct> GetProductByNo(string productNo);
        Task CreateProduct(CatalogProduct product);
        Task UpdateProduct(CatalogProduct product);
        Task DeleteProduct(long id);
    }
}
