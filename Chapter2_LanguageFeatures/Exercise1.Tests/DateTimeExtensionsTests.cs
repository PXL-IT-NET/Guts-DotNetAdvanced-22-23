using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Exercise1.Tests;

[ExerciseTestFixture("dotNet2", "H02", "Exercise01", @"Exercise1\DateTimeExtensions.cs")]
public class DateTimeExtensionsTests
{
    private TypeInfo _dateTimeExtensionsTypeInfo;
    private MethodInfo _toCenturyMethodInfo;

    [OneTimeSetUp]
    public void BeforeAllTests()
    {
        _dateTimeExtensionsTypeInfo = typeof(DateTimeExtensions).GetTypeInfo();

        if (_dateTimeExtensionsTypeInfo == null) return;

        _toCenturyMethodInfo = _dateTimeExtensionsTypeInfo.DeclaredMethods.FirstOrDefault(m => m.Name == "ToCentury");
    }

    [MonitoredTest("DateTimeExtensions - Should be a class that can contain extension methods"), Order(1)]
    public void _01_ShouldBeAClassThatCanContainExtensionMethods()
    {
        Assert.That(_dateTimeExtensionsTypeInfo.IsAbstract && _dateTimeExtensionsTypeInfo.IsSealed, Is.True, "The 'DateTimeExtensions' class must be static");
    }


    [MonitoredTest("DateTimeExtensions - ToCentury - Should be defined correctly"), Order(3)]
    public void _02_ToCentury_ShouldBeDefinedCorrectly()
    {
        AssertToCenturyMethodIsDefinedCorrectly();
    }

    [MonitoredTest("DateTimeExtensions - ToCentury - Should return correct century"), Order(4)]
    [TestCase("20/12/1980", "20")]
    [TestCase("01/01/2000", "20")]
    [TestCase("10/10/2001", "21")]
    public void _03_ToCentury_ShouldReturnCorrectCentury(string date, string expected)
    {
        AssertToCenturyMethodIsDefinedCorrectly();

        string? result = InvokeToCentury(DateTime.Parse(date, CultureInfo.CreateSpecificCulture("nl-be")));
        Assert.That(result, Is.EqualTo(expected));
    }

    private string? InvokeToCentury(DateTime date)
    {
        return _toCenturyMethodInfo.Invoke(null, new object[] { date }) as string;
    }

    private void AssertToCenturyMethodIsDefinedCorrectly()
    {
        Assert.That(_toCenturyMethodInfo, Is.Not.Null, "Cannot find a method with the name 'ToCentury'.");
        Assert.That(_toCenturyMethodInfo.IsStatic, Is.True, "The 'ToCentury' method must be static");
        Assert.That(_toCenturyMethodInfo.ReturnParameter.ParameterType, Is.EqualTo(typeof(string)),
            "The method should return a 'string'.");

        var parameters = _toCenturyMethodInfo.GetParameters();

        var dateTimeParameter = parameters.FirstOrDefault(p => p.ParameterType == typeof(DateTime));
        Assert.That(dateTimeParameter, Is.Not.Null, "Cannot find a parameter of type 'DateTime'.");

        Assert.That(_toCenturyMethodInfo.GetCustomAttribute<ExtensionAttribute>(), Is.Not.Null,
            "You must use the 'this' keyword for the method to be an extension method");
    }
}