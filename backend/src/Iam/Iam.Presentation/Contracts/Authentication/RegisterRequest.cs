namespace Iam.Presentation.Contracts.Authentication;

public sealed record RegisterRequest
{
	public string Email { get; init; } = default!;
	public string Password { get; init; } = default!;
}
