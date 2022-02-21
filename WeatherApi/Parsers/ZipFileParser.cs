using System.IO.Compression;

namespace WeatherApi.Parsers;

public class ZipFileParser<T> : ICompressedFileParser<T>
{
    public IEnumerable<T>? ParseCompressedFile(Stream stream, string compressedFilename, IFileParser<T> fileParser)
    {
        IEnumerable<T>? result = null;

        using ZipArchive package = new ZipArchive(stream, ZipArchiveMode.Read);
        var historicalFile = package.Entries.FirstOrDefault(entry => entry.Name.Equals(compressedFilename));

        if (historicalFile != null)
        {
            using (var zipStream = historicalFile.Open())
            {
                result = fileParser.ParseFile(zipStream);
            }
        }

        return result;
    }
}
