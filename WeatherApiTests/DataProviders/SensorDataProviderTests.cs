using DataServices.Readers;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using WeatherApi.DataProviders;
using WeatherApi.Models;
using WeatherApi.Models.Extensions;
using WeatherApi.Parsers;
using Xunit;

namespace WeatherApiTests.DataProviders
{
    public class SensorDataProviderTests
    {
        private readonly IStorageReader _storageReader;
        private readonly IFileParser<SensorMeasurement> _fileParser;
        private readonly IDataProvider<SensorMeasurementsRequest, IEnumerable<SensorMeasurement>?> _nextHandler;

        private readonly SensorDataProvider _sensorDataProvider;

        private readonly DateTime dateTime = new DateTime(2020, 1, 1);

        public SensorDataProviderTests()
        {
            _storageReader = Substitute.For<IStorageReader>();
            _fileParser = Substitute.For<IFileParser<SensorMeasurement>>();
            _nextHandler = Substitute.For<IDataProvider<SensorMeasurementsRequest, IEnumerable<SensorMeasurement>?>>();

            _sensorDataProvider = new SensorDataProvider(_storageReader, _fileParser, _nextHandler);
        }

        [Fact]
        public async Task WhenThereIsNoFileAndNextHandlersReturnNull_HandleAsync_ShouldReturnNull()
        {
            // Arrange
            var request = new SensorMeasurementsRequest("testSensor", dateTime, "testType");

            // Act
            var result = await _sensorDataProvider.HandleAsync(request);

            // Assert
            result.Should().BeNullOrEmpty();
            _nextHandler.Received().HandleAsync(request);
        }

        [Fact]
        public async Task WhenThereIsNoFileAndNextHandlersReturnResult_HandleAsync_ShouldReturnResult()
        {
            // Arrange
            var request = new SensorMeasurementsRequest("testSensor", dateTime, "testType");
            var measurement = new SensorMeasurement { Time = dateTime, Value = 34.3F };
            var response = new List<SensorMeasurement>() { measurement };
            _nextHandler.HandleAsync(request).Returns(response);

            // Act
            var result = await _sensorDataProvider.HandleAsync(request);

            // Assert
            result.Should().Contain(measurement);
            _nextHandler.Received().HandleAsync(request);
        }

        [Fact]
        public async Task WhenFileExistsButParserFails_HandleAsync_ShouldReturnNull()
        {
            // Arrange
            var request = new SensorMeasurementsRequest("testSensor", dateTime, "testType");
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("test"));
            _storageReader.ReadFileAsync("testFile").Returns(stream);

            // Act
            var result = await _sensorDataProvider.HandleAsync(request);

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task WhenFileExistsAndParserReturnsResult_HandleAsync_ShouldResult()
        {
            // Arrange
            var request = new SensorMeasurementsRequest("testSensor", dateTime, "testType");
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("test"));
            var measurement = new SensorMeasurement { Time = dateTime, Value = 34.3F };
            var response = new List<SensorMeasurement>() { measurement };

            _storageReader.ReadFileAsync(request.GetMeasurementsFilePath()).Returns(stream);
            _fileParser.ParseFile(stream).Returns(response);

            // Act
            var result = await _sensorDataProvider.HandleAsync(request);

            // Assert
            result.Should().Contain(measurement);
            _nextHandler.DidNotReceive().HandleAsync(request);
        }
    }
}