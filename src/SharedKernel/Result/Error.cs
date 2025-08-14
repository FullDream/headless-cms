namespace SharedKernel.Result;

public sealed record Error(string Code, string Message, string? Property = null, ErrorType Type = ErrorType.Failure);