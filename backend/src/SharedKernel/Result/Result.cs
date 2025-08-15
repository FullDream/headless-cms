using System.Diagnostics.CodeAnalysis;

namespace SharedKernel.Result;

public class Result<T> : IFailureFactory<Result<T>>
{
	private Result(T value)
	{
		IsSuccess = true;
		Value = value;
	}

	private Result(params Error[] errors)
	{
		IsSuccess = false;
		Errors = errors;
	}


	[MemberNotNullWhen(true, nameof(Value))]
	[MemberNotNullWhen(false, nameof(Errors))]
	public bool IsSuccess { get; }

	[MemberNotNullWhen(true, nameof(Errors))]
	[MemberNotNullWhen(false, nameof(Value))]
	public bool IsFailure => !IsSuccess;

	public T? Value { get; }
	public Error[]? Errors { get; } = null;
	public static Result<T> Failure(params Error[] errors) => new Result<T>(errors);


	public static Result<T> Success(T value) => new Result<T>(value);

	public static implicit operator Result<T>(T value) => Success(value);
	public static implicit operator Result<T>(Error error) => Failure(error);
	public static implicit operator Result<T>(Error[] error) => Failure(error);
}