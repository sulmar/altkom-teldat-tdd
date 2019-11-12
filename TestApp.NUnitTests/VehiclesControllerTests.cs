using Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.NUnitTests
{
    public class VehiclesControllerTests
    {
        private VehiclesController vehiclesController;

        [SetUp]
        public void Setup()
        {
            vehiclesController = new VehiclesController();
        }


        [Test]
        public void Get_IdIsZero_ReturnNotFound()
        {
            var result = vehiclesController.Get(0);

            // Conrete type
            Assert.That(result, Is.TypeOf<NotFoundResult>());

            // Derrived
            Assert.That(result, Is.InstanceOf<ActionResult>());

            // Concrete type
            result.Should().BeOfType<NotFoundResult>();

            // Derrived


        }

        [Test]
        public void Get_IdIsNotZero_ReturnOk()
        {

        }
    }
}
