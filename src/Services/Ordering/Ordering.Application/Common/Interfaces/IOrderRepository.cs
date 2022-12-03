using Ordering.Domain.Entities;
using Contracts.Common.Interfaces;

namespace Ordering.Application.Common.Interfaces
{
    public interface IOrderRepository : IRepsoitoryBaseAsync<Order, long>
    {
        Task<IEnumerable<Order>> GetOrderByUserName(string userName);
    }
}
