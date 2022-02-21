using System.Globalization;

namespace WeatherApi.Models.Extensions;

public static class SensorMeasurementsRequestExtensions
{
    public static string GetMeasurementsFilePath(this SensorMeasurementsRequest request)
    {
        Enum.TryParse<SensorType>(request.sensorType, out var sensorType);

        return $"{request.deviceId}/{sensorType.ToString().ToLower()}/{request.GetMeasurementsFileName()}";
    }

    public static string GetMeasurementsFileName(this SensorMeasurementsRequest request)
    {
        return $"{request.date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.csv";
    }

    public static string GetHistoricalMeasurementsFilePath(this SensorMeasurementsRequest request)
    {
        Enum.TryParse<SensorType>(request.sensorType, out var sensorType);

        return $"{request.deviceId}/{sensorType.ToString().ToLower()}/historical.zip";
    }
}
