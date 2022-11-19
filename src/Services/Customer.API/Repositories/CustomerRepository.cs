using Contracts.Common.Interfaces;
using Customer.API.Porsistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories
{
    public class CustomerRepository : RepositoryQueryBase<Entities.Customer, int, CustomerContext>, ICustomerRepository
    {
        public CustomerRepository(CustomerContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Entities.Customer>> GetCustomerAsync() => await FindAll().ToListAsync();

        public Task<Entities.Customer> GetCustomerByUserNameAsync(string userName) =>
            FindByCondition(x => x.UserName.Equals(userName)).SingleOrDefaultAsync();
    }
}
