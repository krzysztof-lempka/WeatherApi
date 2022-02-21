using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace WeatherApi.Parsers;

public class CsvFileParser<T> : IFileParser<T> where T : class
{
    public IEnumerable<T> ParseFile(Stream stream)
    {
        var streamReader = new StreamReader(stream);
        var config = new CsvConfiguration(CultureInfo.GetCultureInfo("pl-PL")) // Comma is used as a float number separator.
        {
            HasHeaderRecord = false,
            Delimiter = ";",
        };

        using (var csv = new CsvReader(streamReader, config))
        {
            return csv.GetRecords<T>().ToList();
        }
    }
}
