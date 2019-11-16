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

        [TestCase("ES", "Estonia")]
        [TestCase("D", "Niemcy")]
        [TestCase("9A", "Chorwacja")]
        [TestCase("S5", "Słowenia")]

        [Test]
        public void Create_ForeingArgument_ReturnFoe(string code, string country)
        {
            var result = VehicleFactory.Create(code);

            Assert.That(result, Is.TypeOf<Foe>());
            Assert.That(((Foe)result).Country, Is.EqualTo(country));
        }

        [Test]
        public void Create_UfoArgument_ThrowNotSupportedException()
        {
            Assert.That(()=> VehicleFactory.Create("XX"), Throws.TypeOf<NotSupportedException>());


        }
    }
}
