using FluentValidation;
using MediatR;
using ValidationException = Ordering.Application.Common.Exceptions.ValidationException;

namespace Ordering.Application.Common.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validator;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validator = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validator.Any()) return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validator.Select(x => x.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .Where(x=>x.Errors.Any())
                .SelectMany(x=>x.Errors)
                .ToList();

            if(failures.Any())
            {
                throw new ValidationException(failures);
            }
            return await next();
        }
    }
}
