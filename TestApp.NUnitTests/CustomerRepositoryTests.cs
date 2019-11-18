using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp.Fakers;

namespace TestApp.NUnitTests
{
    public class CustomerRepositoryTests
    {

        [Test]
        public void Get_WhenCalled_ReturnsCustomers()
        {
            // Arrange
            ICustomerRepository customerRepository = new CustomerRepository();

            // Act
            var result = customerRepository.Get();

            // Assert
            result.Should().NotBeEmpty();
            result.Should().HaveCount(c => c == 100).And.OnlyHaveUniqueItems();

        }
    }
}
