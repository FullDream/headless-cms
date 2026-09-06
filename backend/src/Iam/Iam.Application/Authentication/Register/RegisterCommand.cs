using BuildingBlocks.Messaging;

namespace Iam.Application.Authentication.Register;

public sealed record RegisterCommand(string Email, string Password) : ICommand;
