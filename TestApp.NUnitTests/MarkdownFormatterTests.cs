using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp.NUnitTests
{
    public class MarkdownFormatterTests
    {
        [Test]
        public void FormatAsBold_ArgumentIsNullOrEmpty()
        {
            var formatter = new MarkdownFormatter();

            Action action = () => formatter.FormatAsBold(string.Empty);

            Assert.That(action, Throws.ArgumentNullException);

            Assert.That(() => formatter.FormatAsBold(string.Empty), Throws.ArgumentNullException);

            Assert.That(() => formatter.FormatAsBold(string.Empty), Throws.Exception.TypeOf<ArgumentNullException>());

            // FluentAssertions
            action.Should().Throw<ArgumentNullException>();


        }

        [Test]
        [TestCase("abc", "**abc**")]
        [TestCase("Lorem ipsum", "**Lorem ipsum**")]
        [TestCase("Lorem ipsum", "**lorem ipsum**")]
        [TestCase(" Lorem ipsum ", "** Lorem ipsum **")] 
        public void FormatAsBold_WhenCalled_ReturnsWithDoubleAsterix(string text, string expected)
        {
            var formatter = new MarkdownFormatter();

            var result = formatter.FormatAsBold(text);


            // specific
            Assert.That(result, Is.EqualTo(expected).IgnoreCase);

            // general
            StringAssert.StartsWith("**", result);
            StringAssert.Contains(text, result);
            StringAssert.EndsWith("**", result);

            // general
            Assert.That(result, Does.StartWith("**"));
            Assert.That(result, Does.Contain(text).IgnoreCase);
            Assert.That(result, Does.EndWith("**"));

            // dotnet add package FluentAssertions
            result.Should()
                .StartWith("**")
                .And
                .EndWith("**")
                .And
                .Contain(text);


        }
    }
}
