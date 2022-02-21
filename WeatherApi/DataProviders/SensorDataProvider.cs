using DataServices.Readers;
using WeatherApi.Models;
using WeatherApi.Models.Extensions;
using WeatherApi.Parsers;

namespace WeatherApi.DataProviders;

public class SensorDataProvider : IDataProvider<SensorMeasurementsRequest, IEnumerable<SensorMeasurement>?>
{
    private readonly IStorageReader _storageReader;
    private readonly IFileParser<SensorMeasurement> _fileParser;

    public IDataProvider<SensorMeasurementsRequest, IEnumerable<SensorMeasurement>?>? Next { get; }

    public SensorDataProvider(IStorageReader storageReader,
        IFileParser<SensorMeasurement> fileParser,
        IDataProvider<SensorMeasurementsRequest, IEnumerable<SensorMeasurement>?>? next = null)
    {
        _storageReader = storageReader;
        _fileParser = fileParser;
        Next = next;
    }

    public async Task<IEnumerable<SensorMeasurement>?> HandleAsync(SensorMeasurementsRequest request)
    {
        IEnumerable<SensorMeasurement>? result = null;

        using (var stream = await _storageReader.ReadFileAsync(request.GetMeasurementsFilePath()))
        {
            if (stream != null)
                result = _fileParser.ParseFile(stream);
        }

        if ((result == null || !result.Any()) && Next != null)
        {
            result = await Next.HandleAsync(request);
        }

        return result;
    }
}