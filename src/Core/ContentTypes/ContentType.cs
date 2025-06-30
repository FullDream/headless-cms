namespace Core.ContentTypes;

public class ContentType(Guid id, string name, ContentTypeKind kind)
{
	public Guid Id { get; private set; } = id;
	public string Name { get; private set; } = name;
	public ContentTypeKind Kind { get; private set; } = kind;
}