using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestApp.Mocking;

namespace TestApp.NUnitTests
{
    public class TrackingServiceTests
    {

        [Test]
        public void Get_ValidJsonLocation_ReturnsLocation()
        {
            // Arrange
            IFileReader fileReader = new FakeValidFileReader();
            TrackingService trackingService = new TrackingService(fileReader);

            // Act
            var result = trackingService.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Latitude, Is.EqualTo(53.125));
            Assert.That(result.Longitude, Is.EqualTo(18.011111));

        }

        [Test]
        public void Get_ValidJsonLocation_ReturnsLocation2()
        {
            // Arrange
            var mockFileReader = new Mock<IFileReader>();

            mockFileReader
                .Setup(fr => fr.Get(It.IsAny<string>()))
                .Returns("{\"latitude\":53.125,\"longitude\":18.011111}");

            IFileReader fileReader = mockFileReader.Object;

            TrackingService trackingService = new TrackingService(fileReader);

            // Act
            var result = trackingService.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Latitude, Is.EqualTo(53.125));
            Assert.That(result.Longitude, Is.EqualTo(18.011111));

        }

        [Test]
        public void Get_InvalidJsonLocation_ThrowsApplicationException()
        {
            // Arrange
            IFileReader fileReader = new FakeInvalidFileReader();
            TrackingService trackingService = new TrackingService(fileReader);
        }



        [Test]
        public async Task Method_Scenario_ExpectedBehavior()
        {
            var result = await DoWorksAsync();

            Assert.That(result, Is.EqualTo("a"));

        }

        public Task<string> DoWorksAsync()
        {
            return Task.FromResult("a");
        }
    }
}
