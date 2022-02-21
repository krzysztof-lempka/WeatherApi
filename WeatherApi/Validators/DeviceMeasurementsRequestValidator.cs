using FluentValidation;
using WeatherApi.Models;

namespace WeatherApi.Validators;

public class DeviceMeasurementsRequestValidator : AbstractValidator<DeviceMeasurementsRequest>
{
    private readonly DateTime _minimumDate = new DateTime(2000, 1, 1);

    public DeviceMeasurementsRequestValidator()
    {
        RuleFor(r => r.deviceId).NotEmpty();
        RuleFor(r => r.date).GreaterThan(_minimumDate);
    }
}