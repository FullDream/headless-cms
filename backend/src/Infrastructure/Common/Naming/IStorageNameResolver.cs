namespace Infrastructure.Common.Naming;

public interface IStorageNameResolver
{
	string Resolve(string contentTypeName);
}