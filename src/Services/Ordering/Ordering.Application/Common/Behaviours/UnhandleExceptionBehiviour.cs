﻿using MediatR;

namespace Ordering.Application.Common.Behaviours
{
    public class UnhandleExceptionBehiviour<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandleExceptionBehiviour(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogError(ex, "Application Rqeuest: Unhandled Exception for Request {Name} {@Request}",
                    requestName, request);
                throw;
            }
        }
    }
}
