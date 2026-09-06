using BuildingBlocks.Messaging;

namespace Iam.Application.Authentication.Login;

public sealed record LoginCommand(string Username, string Password) : ICommand;
