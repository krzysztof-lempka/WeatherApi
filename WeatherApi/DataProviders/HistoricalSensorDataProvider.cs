using DataServices.Readers;
using WeatherApi.Models;
using WeatherApi.Models.Extensions;
using WeatherApi.Parsers;

namespace WeatherApi.DataProviders;

public class HistoricalSensorDataProvider : IDataProvider<SensorMeasurementsRequest, IEnumerable<SensorMeasurement>?>
{
    private readonly IStorageReader _storageReader;
    private readonly IFileParser<SensorMeasurement> _fileParser;
    private readonly ICompressedFileParser<SensorMeasurement> _compressedFileParser;

    public IDataProvider<SensorMeasurementsRequest, IEnumerable<SensorMeasurement>?>? Next { get; init; }

    public HistoricalSensorDataProvider(IStorageReader storageReader,
        IFileParser<SensorMeasurement> fileParser,
        ICompressedFileParser<SensorMeasurement> compressedFileParser,
        IDataProvider<SensorMeasurementsRequest, IEnumerable<SensorMeasurement>?>? next = null)
    {
        _storageReader = storageReader;
        _fileParser = fileParser;
        _compressedFileParser = compressedFileParser;
        Next = next;
    }

    public async Task<IEnumerable<SensorMeasurement>?> HandleAsync(SensorMeasurementsRequest request)
    {
        IEnumerable<SensorMeasurement>? result = null;

        var stream = await _storageReader.ReadFileAsync(
            request.GetHistoricalMeasurementsFilePath()
        );

        if (stream != null)
        {
            result = _compressedFileParser.ParseCompressedFile(stream, request.GetMeasurementsFileName(), _fileParser);
        }

        if ((result == null || !result.Any()) && Next != null)
        {
            result = await Next.HandleAsync(request);
        }

        return result;
    }
}