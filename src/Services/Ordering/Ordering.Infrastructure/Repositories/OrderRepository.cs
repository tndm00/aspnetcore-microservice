using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders;
using Ordering.Application.Features.V1.Orders.Commands.UpdateOrder;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBaseAsync<Order, long, OrderContext>, IOrderRepository
    {
        public OrderRepository(OrderContext orderContext, IUnitOfWork<OrderContext> unitOfWork)
            : base(orderContext, unitOfWork)
        {

        }

        public void CreateOrder(Order crateObject) => Create(crateObject);

        public async Task DeleteOrder(long id)
        {
            var order = await GetByIdAsync(id);
            if (order != null) await DeleteAsync(order);
        }

        public async Task<IEnumerable<Order>> GetOrderByUserName(string userName) =>
            await FindByCondition(x => x.UserName.Equals(userName)).ToListAsync();

        public async Task<Order> UpdateOrder(Order updateObject)
        {
            await UpdateAsync(updateObject);
            return updateObject;
        }
    }
}
