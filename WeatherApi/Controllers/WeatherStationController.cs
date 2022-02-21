using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi.Controllers;

[ApiController]
[Route("v1/devices")]
public class WeatherStationController : ControllerBase
{
    private readonly AbstractValidator<SensorMeasurementsRequest> _sensorMeasurementsRequestValidator;
    private readonly AbstractValidator<DeviceMeasurementsRequest> _deviceMeasurementsRequestValidator;
    private readonly IWeatherDataService _weatherDataService;

    public WeatherStationController(AbstractValidator<SensorMeasurementsRequest> sensorMeasurementsRequestValidator,
        AbstractValidator<DeviceMeasurementsRequest> deviceMeasurementsRequestValidator,
        IWeatherDataService weatherDataService)
    {
        _sensorMeasurementsRequestValidator = sensorMeasurementsRequestValidator;
        _deviceMeasurementsRequestValidator = deviceMeasurementsRequestValidator;
        _weatherDataService = weatherDataService;
    }

    [HttpGet]
    [Route("GetMeasurements")]
    public async Task<ActionResult<IEnumerable<SensorMeasurement>>> GetMeasurements(string deviceId, DateTime date, string sensorType)
    {
        var request = new SensorMeasurementsRequest(deviceId, date, sensorType);
        var validationResult = _sensorMeasurementsRequestValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var measurements = await _weatherDataService.GetSensorMeasurementsAsync(request);

        return measurements != null ? Ok(measurements) : NotFound();
    }

    [HttpGet]
    [Route("GetMeasurementsForDevice")]
    public async Task<ActionResult<WeatherData>> GetMeasurementsForDevice(string deviceId, DateTime date)
    {
        var request = new DeviceMeasurementsRequest(deviceId, date);
        var validationResult = _deviceMeasurementsRequestValidator.Validate(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var weatherData = await _weatherDataService.GetAllMeasurementsFromDeviceAsync(request);

        return weatherData != null ? Ok(weatherData) : NotFound();
    }
}