using FluentValidation;
using WeatherApi.Models;

namespace WeatherApi.Validators;

public class SensorMeasurementsRequestValidator : AbstractValidator<SensorMeasurementsRequest>
{
    private readonly DateTime _minimumDate = new DateTime(2000, 1, 1);

    public SensorMeasurementsRequestValidator()
    {
        RuleFor(r => r.deviceId).NotEmpty();
        RuleFor(r => r.date).GreaterThan(_minimumDate);
        RuleFor(r => r.sensorType).Must(BeAValidSensorType);
    }

    private bool BeAValidSensorType(string sensorType)
    {
        return Enum.TryParse(sensorType, out SensorType _);
    }
}
