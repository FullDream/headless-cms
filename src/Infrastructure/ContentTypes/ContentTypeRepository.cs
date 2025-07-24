using Core.ContentTypes;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ContentTypes;

public class ContentTypeRepository(AppDbContext dbContext) : IContentTypeRepository
{
	public async Task<IEnumerable<ContentType>> FindManyAsync(ContentTypeKind? kind = null,
		CancellationToken cancellationToken = default)
	{
		return await dbContext.ContentTypes
			.Include(ct => ct.Fields)
			.AsNoTracking()
			.Where(ct => kind == null || ct.Kind == kind)
			.ToListAsync(cancellationToken);
	}

	public async Task<ContentType?> FindByNameAsync(string name, CancellationToken cancellationToken = default)
	{
		return await dbContext.ContentTypes
			.Include(ct => ct.Fields)
			.AsNoTracking()
			.FirstOrDefaultAsync(ct => ct.Name == name, cancellationToken);
	}

	public async Task<ContentType?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
	{
		return await dbContext.ContentTypes.FindAsync([id], cancellationToken);
	}

	public async Task<ContentType> AddAsync(ContentType contentType, CancellationToken cancellationToken = default)
	{
		dbContext.ContentTypes.Add(contentType);
		await dbContext.SaveChangesAsync(cancellationToken);

		return contentType;
	}

	public async Task<ContentType> UpdateAsync(ContentType contentType, CancellationToken cancellationToken = default)
	{
		dbContext.ContentTypes.Update(contentType);
		await dbContext.SaveChangesAsync(cancellationToken);

		return contentType;
	}

	public async Task<ContentType?> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var entity = await dbContext.ContentTypes.FindAsync([id], cancellationToken);

		if (entity == null) return null;

		dbContext.ContentTypes.Remove(entity);
		await dbContext.SaveChangesAsync(cancellationToken);

		return entity;
	}
}