using CsvHelper.Configuration.Attributes;

namespace WeatherApi.Models;

public class SensorMeasurement
{
    [Index(0)]
    public DateTime? Time { get; set; }
    
    [Index(1)]
    public float? Value { get; set; }
}
