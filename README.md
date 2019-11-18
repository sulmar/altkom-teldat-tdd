# TDD

## MSTest

### Szablon testu

~~~ csharp
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
}
~~~


### Walidacja wyniku

~~~ csharp
[TestClass]
  public class RentTests
  {
      [TestMethod]
      public void CanReturn_UserIsAdmin_ResurnsTrue()
      {
          // Arrange
          var rent = new Rent();

          // Act
          var result = rent.CanReturn(new User { IsAdmin = true });

          // Assert
          Assert.IsTrue(result);
      }

      [TestMethod]
      public void CanReturn_SameUser_ReturnsTrue()
      {
          // Arrange
          var user = new User();
          var rent = new Rent() { Rentee = user };

          // Act
          var result = rent.CanReturn(user);

          // Assert
          Assert.IsTrue(result);
      }

      [TestMethod]
      public void CanReturn_AnotherUser_ReturnsFalse()
      {
          // Arrange
          var user = new User();
          var rent = new Rent() { Rentee = user };

          // Act
          var result = rent.CanReturn(new User());

          // Assert
          Assert.IsFalse(result);
      }
  }
~~~

### Wyjątki

~~~ csharp
[ExpectedException(typeof(ArgumentNullException)]
[TestMethod]
public void ExceptionTest()
{
    // Arrange
    Order order = null;
    IOrderCalculator orderCalculator = new MyOrderCalculator();

    // Act
    Action act = () => orderCalculator.CalculateDiscount(order);

}
~~~


## NUnit

### Walidacja wyniku

~~~ csharp
 public class MathCalculatorTests
  {
      private MathCalculator mathCalculator;

      [SetUp]
      public void Setup()
      {
          mathCalculator = new MathCalculator();
      }

      [Test]
      public void Add_WhenCalled_ReturnsTheSumOfArguments()
      {
          var mathCalculator = new MathCalculator();

          var result = mathCalculator.Add(1, 2);

          Assert.That(result, Is.EqualTo(3));

      }

      [Test]
      public void Max_FirstArgumentIsGreater_ReturnTheFirstArgument()
      {
          var mathCalculator = new MathCalculator();

          var result = mathCalculator.Max(2, 1);

          Assert.That(result, Is.EqualTo(2));
      }


      [Test]
      public void Max_SecondArgumentIsGreater_ReturnTheSecondArgument()
      {
          var mathCalculator = new MathCalculator();

          var result = mathCalculator.Max(1, 2);

          Assert.That(result, Is.EqualTo(2));
      }

      [Test]
      public void Max_ArgumentsAreEqual_ReturnTheSameArgument()
      {
          var mathCalculator = new MathCalculator();

          var result = mathCalculator.Max(1, 1);

          Assert.That(result, Is.EqualTo(1));
      }
  }
    
~~~    

### Parametryzacja przypadków testowych

~~~ csharp

 public class MathCalculatorTests
{
    private MathCalculator mathCalculator;

    [SetUp]
    public void Setup()
    {
        mathCalculator = new MathCalculator();
    }

    [Test]
    [TestCase(2, 1, 2)]
    [TestCase(1, 2, 2)]
    [TestCase(1, 1, 1)]
    public void Max_WhenCalled_ReturnTheGreaterArgument(int a, int b, int expected)
    {
        var result = mathCalculator.Max(a, b);

        Assert.That(result, Is.EqualTo(expected));
    }
}
    
~~~

## Testowanie String

~~~ csharp

public class MarkdownFormatterTests
{
    [Test]
    public void FormatAsBold_WhenCalled_ShouldEncloseStringWithDoubleAsterix()
    {
        var formatter = new MarkdownFormatter();

        var result = formatter.FormatAsBold("abc");

        // Specific
        Assert.That(result, Is.EqualTo("**abc**").IgnoreCase);

        Assert.That(result, Does.StartWith("**"));
        Assert.That(result, Does.Contain("abc"));
        Assert.That(result, Does.EndWith("**"));

    }
~~~


## Walidacja kolekcji

~~~ csharp
[Test]
public void GetPrimeNumbers_LimitAsGreaterThanZero_ReturnPrimeNumbersUpToLimit()
{
    var result = mathCalculator.GetPrimeNumbers(100);

    Assert.That(result, Is.Not.Empty);
    Assert.That(result.Count(), Is.EqualTo(25));

    Assert.That(result, Does.Contain(2));
    Assert.That(result, Does.Contain(3));
    Assert.That(result, Does.Contain(5));


    Assert.That(result, Is.EquivalentTo(new[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 }));

    Assert.That(result, Is.Ordered);
    Assert.That(result, Is.Unique);
}
~~~


## Testowanie zwracanego typu

~~~ csharp

public class VehiclesControllerTests
{
    [Test]
    public void Get_IdIsZero_ReturnNotFound()
    {
        var controller = new VehiclesController();

        var result = controller.Get(0);

        Assert.That(result, Is.TypeOf<NotFoundResult>());

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void Get_IdIsNotZero_ReturnOk()
    {
        var controller = new VehiclesController();

        var result = controller.Get(1);

        Assert.That(result, Is.TypeOf<OkResult>());
    }
}

  ~~~


## Walidacja metod void

~~~ csharp
public class LoggerTests
{
    [Test]
    public void Log_WhenCalled_SetLastMessageProperty()
    {
        var logger = new Logger();

        logger.Log("a");

        Assert.That(logger.LastMessage, Is.EqualTo("a"));
    }
}
~~~

## Walidacja wyjątków

~~~ csharp

[Test]
[TestCase(null)]
[TestCase("")]
[TestCase(" ")]
public void Log_EmptyMessage_ThrowArgumentNullException(string message)
{
    var logger = new Logger();

   // logger.Log(message);

    Assert.That(()=>logger.Log(message), Throws.ArgumentNullException);
    Assert.That(() => logger.Log(message), Throws.Exception.TypeOf<ArgumentNullException>());
}
~~~

## Walidacja zdarzeń

~~~ csharp
 [Test]
  public void Log_ValidMessage_RaiseMessageLoggedEvent()
  {
      var logger = new Logger();

      var id = DateTime.MinValue;

      logger.MessageLogged += (sender, args) => { id = args; };

      logger.Log("a");

      Assert.That(id, Is.Not.EqualTo(DateTime.MinValue));
  }
~~~
