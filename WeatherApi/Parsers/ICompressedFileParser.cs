namespace WeatherApi.Parsers;

public interface ICompressedFileParser<T>
{
    IEnumerable<T>? ParseCompressedFile(Stream stream, string compressedFilename, IFileParser<T> parser);
}
