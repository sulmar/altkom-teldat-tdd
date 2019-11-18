using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.NUnitTests
{
    public class DiscountCalculatorTests
    {
        private readonly static DateTime BlackFriday = DateTime.Parse("2019-11-29");

        [Test]
        public void CalculateDiscount_OrderEmpty_ThrowArgumentNullException()
        {
            BlackFridayDiscountCalculator discountCalculator = new BlackFridayDiscountCalculator(0m);


            discountCalculator.CalculateDiscount(null);

        }

        [Test]
        [TestCase(0.5, 50)]
        [TestCase(0.25, 25)]
        public void CalculateDiscount_DateIsBlackFriday_ReturnsDiscountTotalAmount
            (
            decimal percentage,
            decimal expected)
        {
            Order order = OrderCase();

            order.OrderedDate = BlackFriday;

            BlackFridayDiscountCalculator discountCalculator = new BlackFridayDiscountCalculator(percentage);

            decimal result = discountCalculator.CalculateDiscount(order);

            Assert.That(result, Is.EqualTo(expected));

        }

        [Test]
        [TestCaseSource("OrderCase")]
        public void CalculateDiscount_DateIsNotBlackFriday_ReturnsNotDiscountTotalAmount(Order order)
        {
            order.OrderedDate = BlackFriday.AddDays(-1);

            BlackFridayDiscountCalculator discountCalculator = new BlackFridayDiscountCalculator(0.5m);

            decimal result = discountCalculator.CalculateDiscount(order);

            Assert.That(result, Is.EqualTo(0m));
        }

        public Order OrderCase()
        {
            return new Order
            {
                OrderedDate = DateTime.Now,
                Details = new List<OrderDetail>
            {
                new OrderDetail(100, 1)
            }
            };
        }  

    }
}
