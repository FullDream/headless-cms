using Humanizer;
using Infrastructure.Common.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Common.Naming;

public class SnakeCaseStorageNameResolver(IOptions<ContentStorageOptions> options) : IStorageNameResolver
{
	private readonly string prefix = options.Value.StoragePrefix;

	public string Resolve(string contentTypeName) => $"{prefix}{contentTypeName.Underscore()}";
}