namespace Customer.API.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IResult> GetCustomerByUserName(string userName);

        Task<IResult> GetCustomerAsync();
    }
}
