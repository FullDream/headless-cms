using MediatR;
using SharedKernel.Result;

namespace Application.Common.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;