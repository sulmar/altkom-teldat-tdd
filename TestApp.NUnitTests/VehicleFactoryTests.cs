using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TestApp.Fundamentals;

namespace TestApp.NUnitTests
{
    public class VehicleFactoryTests
    {
        [Test]
        public void Create_SPArgument_ReturnFriend()
        {
            var result = VehicleFactory.Create("SP");

            Assert.That(result, Is.TypeOf<Friend>());
        }

        [Test]
        public void Create_ForeingArgument_ReturnFoe()
        {
            var result = VehicleFactory.Create("ES");

            Assert.That(result, Is.TypeOf<Foe>());
            Assert.That(((Foe)result).Country, Is.EqualTo("Estonia"));
        }

        [Test]
        public void Create_UfoArgument_ThrowNotSupportedException()
        {
            Assert.That(()=> VehicleFactory.Create("XX"), Throws.TypeOf<NotSupportedException>());


        }
    }
}
