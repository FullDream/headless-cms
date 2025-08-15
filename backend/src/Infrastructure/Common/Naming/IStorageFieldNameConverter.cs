namespace Infrastructure.Common.Naming;

public interface IStorageFieldNameConverter
{
	public string FromStorage(string name);
	public string ToStorage(string name);
}