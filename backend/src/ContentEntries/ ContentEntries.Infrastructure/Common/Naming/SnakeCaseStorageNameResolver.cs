using ContentEntries.Infrastructure.Common.Configuration;
using Humanizer;
using Microsoft.Extensions.Options;

namespace ContentEntries.Infrastructure.Common.Naming;

public class SnakeCaseStorageNameResolver(IOptions<ContentStorageOptions> options) : IStorageNameResolver
{
	private readonly string prefix = options.Value.StoragePrefix;

	public string Resolve(string contentTypeName) => $"{prefix}{contentTypeName.Underscore()}";
}