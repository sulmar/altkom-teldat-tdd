using FluentAssertions;
using NUnit.Framework;
using System;

namespace TestApp.NUnitTests
{
    public class HappyHoursCanDiscountStrategyTests
    {
        [Test]
        [TestCase("09:00:00", "16:30:00", "2019-01-01 09:30:00", true)]
        [TestCase("09:00:00", "16:30:00", "2019-01-01 16:30:00", true)]
        [TestCase("09:00:00", "16:30:00", "2019-01-01 08:59:30:00", false)]
        [TestCase("09:00:00", "16:30:00", "2019-01-01 16:31:00", false)]
        public void CanDiscount_IsHappyHours_ReturnsCanDiscount(TimeSpan from, TimeSpan to, DateTime orderDate, bool expected)
        {
            Order order = new Order { OrderedDate = orderDate };

            // Arrange
            ICanDiscountStrategy canDiscount = new HappyHoursCanDiscountStrategy(from, to);

            // Act
            var result = canDiscount.CanDiscount(order);

            // Assets
            result.Should().Be(expected);
        }


        
    }
}
