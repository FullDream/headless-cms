namespace Core.ContentTypes;

public interface IContentTypeRepository
{
	Task<IEnumerable<ContentType>> FindManyAsync(ContentTypeKind? kind = null,  CancellationToken cancellationToken = default);
	Task<ContentType?> FindByNameAsync(string name,  CancellationToken cancellationToken = default);
	Task<ContentType?> FindByIdAsync(Guid id,  CancellationToken cancellationToken = default);
	Task<ContentType> AddAsync(ContentType contentType,  CancellationToken cancellationToken = default);
	Task<ContentType> UpdateAsync(ContentType contentType,  CancellationToken cancellationToken = default);
	Task<ContentType?> DeleteAsync(Guid id,  CancellationToken cancellationToken = default);
}