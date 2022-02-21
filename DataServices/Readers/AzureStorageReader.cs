using Microsoft.WindowsAzure.Storage;

namespace DataServices.Readers;

public class AzureStorageReader : IStorageReader
{
    private readonly string _connectionString;
    private readonly string _containerName;

    public AzureStorageReader(string connectionString, string containerName)
    {
        _connectionString = connectionString;
        _containerName = containerName;
    }

    public async Task<Stream?> ReadFileAsync(string filename)
    {
        Stream? stream = null;
        var storageAccount = CloudStorageAccount.Parse(_connectionString);
        var blobClient = storageAccount.CreateCloudBlobClient();
        var container = blobClient.GetContainerReference(_containerName);
        try
        {
            var blobReference = container.GetBlobReference(filename);
            stream = await blobReference.OpenReadAsync();
        }
        catch (StorageException ex)
        {
            //_logger.LogError(ex.ToString());
        }

        return stream;
    }
}