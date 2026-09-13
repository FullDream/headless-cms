using Iam.Domain.Users;
using MediatR;
using SharedKernel.Result;

namespace Iam.Application.Users.GetUserById;

internal sealed class GetUserByIdQueryHandler(IUserRepository userRepository)
	: IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
	public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		var user = await userRepository.FindByIdAsync(request.Id, cancellationToken);

		return user is null ? UserErrors.NotFound(nameof(request.Id)) : user.ToDto();
	}
}
