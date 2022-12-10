using AutoMapper;
using MediatR;
using Ordering.Application.Common.Interfaces;
using Ordering.Domain.Entities;
using Shared.SeedWork;
using ILogger = Serilog.ILogger;

namespace Ordering.Application.Features.V1.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandle : IRequestHandler<CreateOrderCommand, ApiResult<long>>
    {
        private readonly IOrderRepository _oderRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public CreateOrderCommandHandle(IOrderRepository orderRepository, 
            ILogger logger, IMapper mapper)
        {
            _oderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        private const string MethodName = "CreateOrderHandler";


        public async Task<ApiResult<long>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"BEGIN: {MethodName}");
            var orderMapping = _mapper.Map<Order>(request);

            await _oderRepository.CreateOrder(orderMapping);
            await _oderRepository.SaveChangesAsync();

            _logger.Information($"END: {MethodName}");

            return new ApiSuccessResult<long>(orderMapping.Id);
        }
    }
}
