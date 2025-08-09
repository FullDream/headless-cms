using Core.ContentEntries;

namespace Infrastructure.ContentEntries;

public interface IContentEntryDataMapper
{
	public ContentEntry Map(IDictionary<string, object?> dict);
}