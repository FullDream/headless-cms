namespace Application.Abstractions;

public interface IContentTypeExistenceChecker
{
	Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
	Task<bool> ExistsByIdAsync(Guid id, CancellationToken ct);
}