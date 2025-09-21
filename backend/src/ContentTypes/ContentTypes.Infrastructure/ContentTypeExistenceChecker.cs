using Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ContentTypes.Infrastructure;

public class ContentTypeExistenceChecker(AppDbContext dbContext) : IContentTypeExistenceChecker
{
	public Task<bool> ExistsByNameAsync(string name, CancellationToken ct) =>
		dbContext.ContentTypes.AnyAsync(type => type.Name == name, ct);

	public Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct) =>
		dbContext.ContentTypes.AnyAsync(x => x.Id == id, ct);
}