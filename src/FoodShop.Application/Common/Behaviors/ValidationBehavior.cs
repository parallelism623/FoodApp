using FluentValidation;
using FluentValidation.Results;
using FoodShop.Contract.Abstraction.Shared;
using MediatR;

namespace FoodShop.Application.Common.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }
            var context = new ValidationContext<TRequest>(request);
            List<ValidationFailure> errors = _validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .Select(x => new ValidationFailure
                (
                    x.PropertyName,
                    x.ErrorMessage
                 ))
                .Distinct()
                .ToList();
            if (errors.Any())
            {
                throw new ValidationException(errors);
            }
            return await next();
        }


    }
}
