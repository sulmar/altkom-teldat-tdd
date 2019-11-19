using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestApp.UnitTests
{
    [TestClass]
    public class RentTests
    {


        [TestMethod]
        public void Method_Scenario_ExpectedBehavior()
        {

            // Arrange

            // Act
        
            // Assert
        }

        // scenario 1
        [TestMethod]
        public void CanReturn_UserIsAdmin_ReturnsTrue()
        {
            // Arrange

            var rent = new Rent();

            // Act
            var result = rent.CanReturn(new User { IsAdmin = true });

            // Assert
            Assert.IsTrue(result);

            
        }

        // scenario 2
        [TestMethod]
        public void CanReturn_UserIsRentee_ReturnsTrue()
        {
            // Arrange
            var user = new User();
            var rent = new Rent { Rentee = user };

            // Act
            var result = rent.CanReturn(user);

            // Assert
            Assert.IsTrue(result);

            
        }

        // scenario 3
        [TestMethod]
        public void CanReturn_UserIsNotAdminOrRentee_ReturnsFalse()
        {
            // Arrange
            var user = new User();
            var rent = new Rent { Rentee = user };

            // Act
            var result = rent.CanReturn(new User());

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanReturn_UserIsEmpty_ArgumentNullException()
        {
            // Arrange
            var rent = new Rent();

            // Act
            var result = rent.CanReturn(null);




        }

       

    }
}
