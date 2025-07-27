namespace Core.ContentTypes;

public interface IContentTypeRepository
{
	Task<IEnumerable<ContentType>> FindManyAsync(ContentTypeKind? kind = null,
		CancellationToken cancellationToken = default);

	Task<ContentType?> FindByNameAsync(string name, CancellationToken cancellationToken = default);
	Task<ContentType?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
	void Add(ContentType contentType);
	void Update(ContentType contentType);
	void Remove(ContentType contentType);
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}