using MediatR;
using SharedKernel.Result;

namespace BuildingBlocks.Messaging;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;