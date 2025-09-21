using ContentEntries.Core;

namespace ContentEntries.Infrastructure;

public interface IContentEntryDataMapper
{
	public ContentEntry Map(IDictionary<string, object?> dict);
}