using BuildingBlocks.Messaging;

namespace Iam.Application.Register;

public sealed record RegisterCommand(string Email, string Password) : ICommand;