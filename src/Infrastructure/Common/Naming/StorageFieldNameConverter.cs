using System.Collections.Concurrent;
using Humanizer;

namespace Infrastructure.Common.Naming;

public class StorageFieldNameConverter : IStorageFieldNameConverter
{
	private readonly ConcurrentDictionary<string, string> camelToSnake = new();
	private readonly ConcurrentDictionary<string, string> snakeToCamel = new();

	public string FromStorage(string name) => snakeToCamel.GetOrAdd(name, s => s.Camelize());

	public string ToStorage(string name) => camelToSnake.GetOrAdd(name, s => s.Underscore());
}