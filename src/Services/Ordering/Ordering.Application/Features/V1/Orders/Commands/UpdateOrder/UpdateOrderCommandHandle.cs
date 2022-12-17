using AutoMapper;
using MediatR;
using Ordering.Application.Common.Exceptions;
using Ordering.Application.Common.Interfaces;
using Ordering.Application.Common.Models;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using ILogger = Serilog.ILogger;

namespace Ordering.Application.Features.V1.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandle : IRequestHandler<UpdateOrderCommand, ApiResult<OrderDto>>
    {
        private readonly IOrderRepository _oderRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public UpdateOrderCommandHandle(IOrderRepository orderRepository,
    ILogger logger, IMapper mapper)
        {
            _oderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private const string MethodName = "UpdateOrderHandler";


        public async Task<ApiResult<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = await _oderRepository.GetByIdAsync(request.Id);
            if (orderEntity is null) throw new NotFoundException(nameof(Order), request.Id);

            _logger.Information($"BEGIN: {MethodName}");
            var orderMapping = _mapper.Map(request, orderEntity);

            await _oderRepository.UpdateOrder(orderMapping);
            await _oderRepository.SaveChangesAsync();
            var result = _mapper.Map<OrderDto>(orderMapping);

            _logger.Information($"END: {MethodName}");

            return new ApiSuccessResult<OrderDto>(result);
        }
    }
}
