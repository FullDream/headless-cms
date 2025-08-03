using Core.ContentTypes;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.ContentTypes;

public class ContentTypeRepository(AppDbContext dbContext) : IContentTypeRepository
{
	public async Task<IEnumerable<ContentType>> FindManyAsync(ContentTypeKind? kind = null,
		CancellationToken cancellationToken = default) =>
		await dbContext.ContentTypes
			.Include(ct => ct.Fields)
			.AsNoTracking()
			.Where(ct => kind == null || ct.Kind == kind)
			.OrderBy(ct => ct.Name)
			.ToListAsync(cancellationToken);

	public Task<ContentType?> FindByNameAsync(string name, CancellationToken cancellationToken = default) =>
		dbContext.ContentTypes
			.Include(ct => ct.Fields)
			.FirstOrDefaultAsync(ct => ct.Name == name, cancellationToken);

	public async Task<ContentType?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
		await dbContext.ContentTypes
			.Include(ct => ct.Fields)
			.FirstOrDefaultAsync(ct => ct.Id == id, cancellationToken);

	public void Add(ContentType contentType) => dbContext.ContentTypes.Add(contentType);
	public void AddField(ContentField field) => dbContext.ContentFields.Add(field);
	public void Update(ContentType contentType) => dbContext.ContentTypes.Update(contentType);

	public void Remove(ContentType contentType) => dbContext.ContentTypes.Remove(contentType);

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
		dbContext.SaveChangesAsync(cancellationToken);
}