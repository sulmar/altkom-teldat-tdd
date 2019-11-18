using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using TestApp;

namespace Tests
{
    public class MathCalculatorTests
    {
        private MathCalculator mathCalculator;

        [SetUp]
        public void Setup()
        {
            mathCalculator = new MathCalculator();
        }


        [Test]
        public void GetPrimeNumbers_LimitIs100_ReturnsPrimeNumbersUpToLimit()
        {

            var result = mathCalculator.GetPrimeNumbers(100).ToList();

            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count(), Is.EqualTo(25));

            Assert.That(result, Does.Contain(2));
            Assert.That(result, Does.Contain(3));
            Assert.That(result, Does.Contain(5));

            Assert.That(result, Is.EquivalentTo(GetExpectedNumbers()));

            Assert.That(result, Is.Ordered);
            Assert.That(result, Is.Unique);

            // Fluent
            result.Should().NotBeEmpty()
                .And.HaveCount(25)
                .And.ContainInOrder(GetExpectedNumbers());





        }


        private static int[] GetExpectedNumbers()
        {
            return new[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
        }

        [Test]
        [TestCase(2, 1, 2)]
        [TestCase(1, 2, 2)]
        [TestCase(1, 1, 1)]
        public void Max_WhenCalled_ReturnsGreaterArgument(int a, int b, int expected)
        {
            // Act
            var result = mathCalculator.Max(a, b);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
       
        [Test]
        [Ignore("No bo tak")]
        public void Max_FirstArgumentIsGreater_ReturnsFirstArgument()
        {
            // Act
            var result = mathCalculator.Max(2, 1);

            Assert.That(result, Is.EqualTo(2));
          //  Assert.That(result == 2);
        }


        [Test]
        [Ignore("No bo tak")]
        public void Max_SecondArgumentIsGreater_ReturnsSecondArgument()
        {
            // Act
            var result = mathCalculator.Max(1, 2);

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        [Ignore("No bo tak")]
        public void Max_FirstAndSecondArgumentIsEqual_ReturnsFirstArgument()
        {
            // Act
            var result = mathCalculator.Max(1, 1);

            Assert.That(result, Is.EqualTo(1));
        }

    }
}