namespace Core.ContentEntries;

public interface IContentEntryRepository
{
	Task<IReadOnlyCollection<ContentEntry>> QueryAsync(string contentTypeName, CancellationToken ct = default);
	Task<ContentEntry?> FindAsync(string contentTypeName, Guid id, CancellationToken ct = default);
	Task<ContentEntry> AddAsync(string contentTypeName, ContentEntry contentEntry, CancellationToken ct = default);
	Task UpdateAsync(string contentTypeName, ContentEntry entry, CancellationToken ct = default);
	Task DeleteAsync(string contentTypeName, Guid id, CancellationToken ct = default);
}