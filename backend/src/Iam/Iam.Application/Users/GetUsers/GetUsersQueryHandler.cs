using Iam.Domain.Users;
using MediatR;
using SharedKernel.Result;

namespace Iam.Application.Users.GetUsers;

internal sealed class GetUsersQueryHandler(IUserRepository userRepository)
	: IRequestHandler<GetUsersQuery, Result<IReadOnlyCollection<UserDto>>>
{
	public async Task<Result<IReadOnlyCollection<UserDto>>> Handle(
		GetUsersQuery request,
		CancellationToken cancellationToken)
	{
		var users = await userRepository.FindManyAsync(cancellationToken);

		return users.Select(user => user.ToDto()).ToArray();
	}
}
