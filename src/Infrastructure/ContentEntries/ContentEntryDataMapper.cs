using Core.ContentEntries;
using Infrastructure.Common.Naming;

namespace Infrastructure.ContentEntries;

public class ContentEntryDataMapper(IStorageFieldNameConverter fieldNameConverter) : IContentEntryDataMapper
{
	public ContentEntry Map(IDictionary<string, object?> dict)
	{
		if (dict["id"] is not string stringId)
			throw new InvalidOperationException("Missing or invalid 'id'");

		if (dict["created_at"] is not string createdAtString)
			throw new InvalidOperationException("Missing or invalid 'created_at'");

		var createdAt = DateTime.Parse(createdAtString);
		var updatedAt = dict["updated_at"] is string uAt ? DateTime.Parse(uAt) : createdAt;

		var fieldValues = dict
			.Where(kv => kv.Key is not "id" and not "created_at" and not "updated_at")
			.ToDictionary(kv => fieldNameConverter.FromStorage(kv.Key), kv => kv.Value);

		return ContentEntry.Hydrate(Guid.Parse(stringId), createdAt, updatedAt, fieldValues);
	}
}