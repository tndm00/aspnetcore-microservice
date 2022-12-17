using AutoMapper;
using MediatR;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using ILogger = Serilog.ILogger;

namespace Ordering.Application.Features.V1.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandle : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _oderRepository;
        private readonly ILogger _logger;

        public DeleteOrderCommandHandle(IOrderRepository orderRepository,
            ILogger logger)
        {
            _oderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private const string MethodName = "DeleteOrderHandler";

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = await _oderRepository.GetByIdAsync(request.Id);
            if (orderEntity is null) throw new NotFoundException(nameof(Order), request.Id);

            _logger.Information($"BEGIN: {MethodName}");

            await _oderRepository.DeleteOrder(request.Id);
            await _oderRepository.SaveChangesAsync();

            _logger.Information($"END: {MethodName}");

            return Unit.Value;
        }
    }
}
