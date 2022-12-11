using Contracts.Interfaces;
using Contracts.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Common.Models;
using Ordering.Application.Features.V1.Orders;
using Ordering.Application.Features.V1.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.V1.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.V1.Orders.Queries.GetOrders;
using Shared.SeedWork;
using Shared.Services.Email;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Ordering.API.Controller
{
    [Route("api/v1/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMessageProducer _messageProducer;

        public OrdersController(IMediator mediator, IMessageProducer messageProducer)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
        }

        private static class RouteNames
        {
            public const string GetOrders = nameof(GetOrders);
            public const string CreateOrder = nameof(CreateOrder);
            public const string UpdateOrder = nameof(UpdateOrder);
            public const string DeleteOrder = nameof(DeleteOrder);
        }

        [HttpGet("{username}", Name = RouteNames.GetOrders)]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByUserName([Required] string username)
        {
            var query = new GetOrdersQuery(username);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost(Name = RouteNames.CreateOrder)]
        [ProducesResponseType(typeof(ApiResult<long>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<long>>> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);

            //Check and Test RabbitMQ
            _messageProducer.SendMessage(result);

            return Ok(result);
        }

        [HttpPut(Name = RouteNames.UpdateOrder)]
        [ProducesResponseType(typeof(ApiResult<OrderDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResult<OrderDto>>> UpdateOrder(long id, [FromBody] UpdateOrderCommand command)
        {
            command.SetId(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete(Name = RouteNames.DeleteOrder)]
        [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteOrder([Required]long id)
        {
            var command = new DeleteOrderCommand(id);
            await _mediator.Send(command);

            return NoContent();
        }
    }
}
