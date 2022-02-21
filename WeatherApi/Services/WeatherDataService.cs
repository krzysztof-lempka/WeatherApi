using WeatherApi.DataProviders;
using WeatherApi.Models;

namespace WeatherApi.Services;

public class WeatherDataService : IWeatherDataService
{
    private IDataProvider<SensorMeasurementsRequest, IEnumerable<SensorMeasurement>?> _sensorDataProvider;

    public WeatherDataService(IDataProvider<SensorMeasurementsRequest, IEnumerable<SensorMeasurement>?> sensorDataProvider)
    {
        _sensorDataProvider = sensorDataProvider;
    }

    public async Task<WeatherData> GetAllMeasurementsFromDeviceAsync(DeviceMeasurementsRequest request)
    {
        var temperatureRequest = new SensorMeasurementsRequest(request.deviceId, request.date, SensorType.Temperature.ToString());
        var humidityRequest = temperatureRequest with { sensorType = SensorType.Temperature.ToString() };
        var rainfallRequest = temperatureRequest with { sensorType = SensorType.Rainfall.ToString() };

        return new WeatherData
        {
            Temperature = await GetSensorMeasurementsAsync(temperatureRequest),
            Humidity = await GetSensorMeasurementsAsync(humidityRequest),
            Rainfall = await GetSensorMeasurementsAsync(rainfallRequest)
        };
    }

    public async Task<IEnumerable<SensorMeasurement>?> GetSensorMeasurementsAsync(SensorMeasurementsRequest request)
    {
        return await _sensorDataProvider.HandleAsync(request);
    }
}