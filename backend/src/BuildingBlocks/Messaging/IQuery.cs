using MediatR;
using SharedKernel.Result;

namespace BuildingBlocks.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;