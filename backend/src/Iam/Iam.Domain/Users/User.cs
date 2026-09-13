namespace Iam.Domain.Users;

public sealed class User
{
	private readonly HashSet<Guid> roleIds = [];


	private User(Guid id, string email, IEnumerable<Guid> roleIds)
	{
		Id = id;
		Email = email;

		this.roleIds.UnionWith(roleIds);
	}

	public Guid Id { get; }

	public string Email { get; private set; }

	public IReadOnlyCollection<Guid> RoleIds => roleIds;

	public static User Restore(Guid id, string email, IEnumerable<Guid> roleIds) => new(id, email, roleIds);

	public void SetRoles(IEnumerable<Guid> roleIds) => this.roleIds.UnionWith(roleIds);

	public void AssignRole(Guid roleId) => roleIds.Add(roleId);

	public void RemoveRole(Guid roleId) => roleIds.Remove(roleId);
}
