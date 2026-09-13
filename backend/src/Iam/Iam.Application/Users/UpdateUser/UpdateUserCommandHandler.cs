using Iam.Domain.Users;
using MediatR;
using SharedKernel.Result;

namespace Iam.Application.Users.UpdateUser;

internal sealed class UpdateUserCommandHandler(IUserRepository userRepository)
	: IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
	public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
	{
		var user = await userRepository.FindByIdAsync(request.Id, cancellationToken);

		if (user is null) return UserErrors.NotFound(nameof(request.Id));

		user.SetRoles(request.RoleIds);

		await userRepository.UpdateAsync(user, cancellationToken);

		return user.ToDto();
	}
}
