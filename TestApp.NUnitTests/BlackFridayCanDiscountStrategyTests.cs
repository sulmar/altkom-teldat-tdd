using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.NUnitTests
{

    public class BlackFridayCanDiscountStrategyTests
    {
        private readonly static DateTime BlackFriday = DateTime.Parse("2019-11-29");

        [Test]
        public void CanDiscount_IsBlackFriday_ReturnsTrue()
        {
            Order order = new Order { OrderedDate = BlackFriday };

            // Arrange
            ICanDiscountStrategy canDiscount = new BlackFridayCanDiscountStrategy();

            // Act
            var result = canDiscount.CanDiscount(order);

            // Assets
            result.Should().BeTrue();
        }


        [Test]
        public void CanDiscount_IsNotBlackFriday_ReturnsFalse()
        {
            Order order = new Order { OrderedDate = BlackFriday.AddDays(-1) };

            // Arrange
            ICanDiscountStrategy canDiscount = new BlackFridayCanDiscountStrategy();

            // Act
            var result = canDiscount.CanDiscount(order);

            // Assets
            result.Should().BeFalse();
        }
    }
}
