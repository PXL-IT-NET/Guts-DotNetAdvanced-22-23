using System.Windows.Data;
using Exercise2.Converters;

namespace Exercise2.Tests;

[ExerciseTestFixture("dotnet2", "H03", "Exercise02",
    @"Exercise2\Converters\RatingStarsConverter.cs;")]
public class RatingStarsConverterTests
{
    private IValueConverter? _converter;

    [SetUp]
    public void BeforeEachTest()
    {
        _converter = new RatingStarsConverter() as IValueConverter;
    }

    [MonitoredTest("RatingStarsConverter - Should be a converter that can be used in bindings")]
    public void _01_ShouldBeAConverterThatCanBeUsedInBindings()
    {
        Assert.That(_converter, Is.Not.Null, "Not a valid converter");
    }

    [MonitoredTest("RatingStarsConverter - Convert - Should convert ratings correctly")]
    [TestCase(1, "*")]
    [TestCase(3, "***")]
    [TestCase(5, "*****")]
    public void _01_Convert_ShouldConvertRatingsCorrectly(int rating, string expected)
    {
        _01_ShouldBeAConverterThatCanBeUsedInBindings();
        string? result = _converter!.Convert(rating, null, null, null) as string;
        Assert.That(result, Is.EqualTo(expected));
    }
}