using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.NUnitTests
{

    public class LoggerTests
    {
        [Test]
        public void Log_WhenCalled_SetLastMessage()
        {
            // Arrange
            var logger = new Logger();

            // Act
            logger.Log("a");

            // Assert
            Assert.That(logger.LastMessage, Is.EqualTo("a"));

        }

        [Test]
        public void Log_WhenCalled_RaiseMessageLoggedEvent()
        {
            // Arrange
            var logger = new Logger();

            var loggedDate = DateTime.MinValue;

            logger.MessageLogged += (sender, args) => { loggedDate = args; };

            // Act
            logger.Log("a");

            // Assert
            Assert.That(loggedDate, Is.Not.EqualTo(DateTime.MinValue));

        }

        [Test]
        public void Log_WhenCalled_RaiseMessageLoggedEvent2()
        {
            // Arrange
            var logger = new Logger();

            using (var monitoredLogger = logger.Monitor())
            {
                // Act
                logger.Log("a");

                // Assert
                monitoredLogger.Should().Raise(nameof(Logger.MessageLogged));

            }
        }
    }
}
