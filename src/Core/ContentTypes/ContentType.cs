namespace Core.ContentTypes;

public class ContentType(Guid id, string name, ContentTypeKind kind)
{
	public Guid Id { get; private set; } = id;
	public string Name { get; private set; } = name;
	public ContentTypeKind Kind { get; private set; } = kind;

	public void Rename(string name)
	{
		Name = name;
	}

	public void ChangeKind(ContentTypeKind kind)
	{
		Kind = kind;
	}
}