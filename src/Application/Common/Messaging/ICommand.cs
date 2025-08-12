using MediatR;
using SharedKernel.Result;

namespace Application.Common.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;