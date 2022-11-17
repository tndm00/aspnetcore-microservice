using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;

namespace Customer.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IResult> GetCustomerByUserName(string userNamae) =>
            Results.Ok(await _customerRepository.GetCustomerByUserNameAsync(userNamae));

        public async Task<IResult> GetCustomerAsync() => 
            Results.Ok(await _customerRepository.GetCustomerAsync());
    }
}
