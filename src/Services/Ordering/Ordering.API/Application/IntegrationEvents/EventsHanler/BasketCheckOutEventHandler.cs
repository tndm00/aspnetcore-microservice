using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Features.V1.Orders;
using ILogger = Serilog.ILogger;

namespace Ordering.API.Application.IntegrationEvents.EventsHanler
{
    public class BasketCheckOutEventHandler : IConsumer<BasketCheckOutEvent>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public BasketCheckOutEventHandler(IMediator mediator, IMapper mapper, ILogger logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        public async Task Consume(ConsumeContext<BasketCheckOutEvent> context)
        {
            var command = _mapper.Map<CreateOrderCommand>(context.Message);
            var result = await _mediator.Send(command);
            _logger.Information("BasketCheckOutEvent consumed successfully. " +
                "Order is created with Id: {newOrderId}", result.Data);
        }
    }
}
