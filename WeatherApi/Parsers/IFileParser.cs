namespace WeatherApi.Parsers;

public interface IFileParser <T>
{
    IEnumerable<T> ParseFile (Stream stream);
}
