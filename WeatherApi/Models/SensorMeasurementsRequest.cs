namespace WeatherApi.Models;

public record SensorMeasurementsRequest(string deviceId, DateTime date, string sensorType);


