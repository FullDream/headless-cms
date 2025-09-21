using FluentValidation;
using MediatR;
using SharedKernel.Result;

namespace Application.Abstractions.Validation;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
	: IPipelineBehavior<TRequest, TResponse>
	where TRequest : notnull
	where TResponse : IFailureFactory<TResponse>
{
	public async Task<TResponse> Handle(
		TRequest request,
		RequestHandlerDelegate<TResponse> next,
		CancellationToken cancellationToken)
	{
		if (validators.Any())
		{
			var context = new ValidationContext<TRequest>(request);
			var results = await Task.WhenAll(
				validators.Select(v => v.ValidateAsync(context, cancellationToken))
			);

			var failures = results
				.SelectMany(r => r.Errors)
				.Where(f => f is not null)
				.ToList();

			if (failures.Count != 0)
			{
				var errors = failures
					.Select(f => new Error(f.ErrorCode, f.ErrorMessage, f.PropertyName, ErrorType.Validation))
					.ToArray();

				return TResponse.Failure(errors);
			}
		}

		return await next(cancellationToken);
	}
}