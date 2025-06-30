namespace Core.ContentTypes;

public interface IContentTypeRepository
{
	Task<IEnumerable<ContentType>> FindManyAsync(ContentTypeKind? kind = null);
	Task<ContentType?> FindByNameAsync(string name);
	Task<ContentType?> FindByIdAsync(Guid id);
	Task<ContentType> AddAsync(ContentType contentType);
	Task<ContentType> UpdateAsync(ContentType contentType);
	Task<ContentType?> DeleteAsync(Guid id);
}