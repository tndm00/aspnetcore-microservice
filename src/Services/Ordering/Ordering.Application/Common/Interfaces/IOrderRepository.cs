using Ordering.Domain.Entities;
using Contracts.Common.Interfaces;
using Ordering.Application.Features.V1.Orders;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders.Commands.UpdateOrder;

namespace Ordering.Application.Common.Interfaces
{
    public interface IOrderRepository : IRepsoitoryBaseAsync<Order, long>
    {
        Task<IEnumerable<Order>> GetOrderByUserName(string userName);
        Task CreateOrder(Order crateObject);
        Task UpdateOrder(Order updateObject);
        Task DeleteOrder(long id);
    }
}
