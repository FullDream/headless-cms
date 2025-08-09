namespace Core.ContentEntries;

public class ContentEntry
{
	private ContentEntry()
	{
		FieldValues = new Dictionary<string, object?>().AsReadOnly();
	}

	public Guid Id { get; private set; } = Guid.NewGuid();
	public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
	public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

	public IReadOnlyDictionary<string, object?> FieldValues { get; private set; }

	public static ContentEntry Create(IReadOnlyDictionary<string, object?> values)
	{
		var now = DateTime.UtcNow;

		return new ContentEntry
		{
			Id = Guid.NewGuid(),
			CreatedAt = now,
			UpdatedAt = now,
			FieldValues = values
		};
	}

	public static ContentEntry Hydrate(Guid id, DateTime createdAt, DateTime updatedAt,
		Dictionary<string, object?> values)
	{
		return new ContentEntry
		{
			Id = id,
			CreatedAt = createdAt,
			UpdatedAt = updatedAt,
			FieldValues = values
		};
	}
}