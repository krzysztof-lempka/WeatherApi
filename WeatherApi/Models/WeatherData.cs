namespace WeatherApi.Models;

public record WeatherData
{
    public IEnumerable<SensorMeasurement>? Temperature { get; init; }
    public IEnumerable<SensorMeasurement>? Humidity { get; init; }
    public IEnumerable<SensorMeasurement>? Rainfall { get; init; }
}