
using AccessControlSystem.SharedKernel.ApiModels;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessControlSystem.SharedKernel.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ServiceResultBase, new()
{
    private const string RequestNotValid = "Request model not valid";

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults =
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(err => err != null)
            .ToList();

        if (failures.Count != 0)
        {
            return new ServiceError(ErrorType.ValidationError, title: RequestNotValid, invalidParameters: failures.Select(f => new InvalidParameter(f.PropertyName, f.ErrorMessage)))
                .Failure<TResponse>();
        }

        return await next();
    }
}