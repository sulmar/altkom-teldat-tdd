using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
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
        public void Get_InvalidJsonLocation_ThrowsApplicationException()
        {
            // Arrange
            IFileReader fileReader = new FakeInvalidFileReader();
            TrackingService trackingService = new TrackingService(fileReader);
        }
    }
}
