using DataServices.Readers;
using FluentValidation;
using WeatherApi.DataProviders;
using WeatherApi.Models;
using WeatherApi.Parsers;
using WeatherApi.Services;
using WeatherApi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();


builder.Services.AddTransient<IWeatherDataService, WeatherDataService>();
builder.Services.AddSingleton<IStorageReader>(x => new AzureStorageReader(
    builder.Configuration["Storage:ConnectionString"],
    builder.Configuration["Storage:ContainerName"],
    x.GetRequiredService<ILogger<AzureStorageReader>>()
));


RegisterValidators(builder.Services);
RegisterFileParsers(builder.Services);
RegisterDataProviders(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void RegisterDataProviders(IServiceCollection services)
{
    services.AddTransient<IDataProvider<SensorMeasurementsRequest, IEnumerable<SensorMeasurement>?>>(
    x => new SensorDataProvider(
        x.GetRequiredService<IStorageReader>(),
        x.GetRequiredService<IFileParser<SensorMeasurement>>(),
        x.GetRequiredService<HistoricalSensorDataProvider>())
    );

    services.AddTransient(x =>
         new HistoricalSensorDataProvider(x.GetRequiredService<IStorageReader>(),
         x.GetRequiredService<IFileParser<SensorMeasurement>>(),
         x.GetRequiredService<ICompressedFileParser<SensorMeasurement>>())
     );
}

static void RegisterFileParsers(IServiceCollection services)
{
    services.AddSingleton(typeof(IFileParser<>), typeof(CsvFileParser<>));
    services.AddSingleton(typeof(ICompressedFileParser<>), typeof(ZipFileParser<>));
}

static void RegisterValidators(IServiceCollection services)
{
    services.AddTransient<AbstractValidator<SensorMeasurementsRequest>, SensorMeasurementsRequestValidator>();
    services.AddTransient<AbstractValidator<DeviceMeasurementsRequest>, DeviceMeasurementsRequestValidator>();
}