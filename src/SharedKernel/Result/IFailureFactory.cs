namespace SharedKernel.Result;

public interface IFailureFactory<TSelf>
	where TSelf : IFailureFactory<TSelf>
{
	static abstract TSelf Failure(Error[] errors);
}