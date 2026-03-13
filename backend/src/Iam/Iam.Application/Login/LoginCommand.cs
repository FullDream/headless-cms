using BuildingBlocks.Messaging;

namespace Iam.Application.Login;

public sealed record LoginCommand(string Username, string Password) : ICommand;