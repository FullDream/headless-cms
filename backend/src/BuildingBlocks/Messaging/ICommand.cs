using MediatR;
using SharedKernel.Result;

namespace BuildingBlocks.Messaging;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;