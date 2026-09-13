namespace Iam.Application.Users;

public sealed record UserDto(Guid Id, string Email, IReadOnlyCollection<Guid> RoleIds);
