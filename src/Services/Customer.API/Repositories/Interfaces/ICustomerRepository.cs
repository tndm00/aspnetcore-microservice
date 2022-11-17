using Contracts.Common.Interfaces;
using Customer.API.Porsistence;

namespace Customer.API.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepositoryQueryBase<Entities.Customer, int, CustomerContext>
    {
        Task<Entities.Customer> GetCustomerByUserNameAsync(string userName);
        Task<IEnumerable<Entities.Customer>> GetCustomerAsync();

    }
}
