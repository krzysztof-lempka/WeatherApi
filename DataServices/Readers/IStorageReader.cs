namespace DataServices.Readers;

public interface IStorageReader
{
    public Task<Stream?> ReadFileAsync(string filename);
}
