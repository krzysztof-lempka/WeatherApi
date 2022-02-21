using WeatherApi.Models;

namespace WeatherApi.Services;

public interface IWeatherDataService
{
    Task<IEnumerable<SensorMeasurement>?> GetSensorMeasurementsAsync(SensorMeasurementsRequest sensorMeasurementsRequest);
    Task<WeatherData> GetAllMeasurementsFromDeviceAsync(DeviceMeasurementsRequest request);
}
