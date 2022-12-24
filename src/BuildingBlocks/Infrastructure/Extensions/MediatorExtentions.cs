using Contracts.Common.Events;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class MediatorExtentions
    {
        public static async Task DispatchDomainEventAsync(this IMediator mediator,
            List<BaseEvent> domainEvents,
            ILogger logger)
        {
            foreach (var item in domainEvents)
            {
                await mediator.Publish(item);
                logger.Information("A domain event has been published!" + $" Event: {item.GetType().Name}");
            }
        }
    }
}
