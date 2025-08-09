namespace Application.Abstractions;

public interface IContentTypeFieldsProvider
{
	Task<ContentFieldsSnapshot?> FindByNameAsync(string typeName, CancellationToken ct);
}