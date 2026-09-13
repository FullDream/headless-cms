using Iam.Domain.Users;

namespace Iam.Application.Users;

internal static class UserMapper
{
	internal static UserDto ToDto(this User user) => new(user.Id, user.Email, user.RoleIds.ToArray());
}
