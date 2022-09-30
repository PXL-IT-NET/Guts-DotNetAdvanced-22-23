using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;
using System.Globalization;

namespace Exercise1.Tests;

[ExerciseTestFixture("dotNet2", "H02", "Exercise01", @"Exercise1\Composition.cs")]
public class CompositionTests
{
    private string _compositionClassContent;

    [OneTimeSetUp]
    public void BeforeAllTests()
    {
        _compositionClassContent = Solution.Current.GetFileContent(@"Exercise1\Composition.cs");
    }

    [MonitoredTest("Composition - Should have Nullable Composer Property")]
    public void _01_ShouldHaveNullableComposerProperty()
    {
        var props = typeof(Composition).GetProperties();
        foreach(var prop in props)
        {
            if(prop.Name=="Composer")
            {
                Assert.That(IsMarkedAsNullable(prop), Is.True, "Composer property has to be nullable"); 
            }
        }
    }

    [MonitoredTest("Composition - Constructor - Should initialize properties")]
    public void _02_Constructor_ShouldInitializeProperties()
    {
        var composition = new Composition();
        Assert.That(composition.Title, Is.Empty, "Constructor should assign an empty string to Title");
        Assert.That(composition.Description, Is.Empty, "Constructor should assign an empty string to Description");
        Assert.That(composition.ReleaseDate, Is.EqualTo(DateTime.MinValue), "Constructor should set the default value (minimum) for the ReleaseDate");
        Assert.That(composition.Composer, Is.Null, "Composer should be null after construction");
    }

    [MonitoredTest("Composition - ToString - Should use the ToCentury Extension method")]
    public void _03_ToString_ShouldUseToCenturyExtensionMethod()
    {
        Assert.That(CallsMemberMethod("ToCentury"), Is.True, "Cannot find an invocation of the 'ToCentury' method of a 'Composition' instance.");
    }

    [MonitoredTest("Composition - ToString - Should return composition info")]
    [TestCase("Some composition", "Some description", "Some composer", "20/12/1980",
        "Title: Some composition\r\nDescription: Some description\r\nComposer: Some composer\r\nRelease: 20/12/1980 -  20° Century")]
    [TestCase("t", "d", null, "15/10/1880",
        "Title: t\r\nDescription: d\r\nComposer: /\r\nRelease: 15/10/1880 -  19° Century")]
    [TestCase("Century end", "d", null, "10/10/1900",
        "Title: Century end\r\nDescription: d\r\nComposer: /\r\nRelease: 10/10/1900 -  19° Century")]
    public void _04_ToString_ShouldReturnCompositionInfo(string title, string description, string composer, string releaseDate, string expected)
    {
        Composition composition = new Composition
        {
            Title = title,
            Description = description,
            ReleaseDate = DateTime.Parse(releaseDate, CultureInfo.CreateSpecificCulture("nl-be"))
        };
        if (composer != null)
        {
            composition.Composer = composer;
        }

        string result = composition.ToString();

        Assert.That(result, Is.EqualTo(expected));
    }

    private bool CallsMemberMethod(string methodName)
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(_compositionClassContent);
        var root = syntaxTree.GetRoot();
        return root
            .DescendantNodes()
            .OfType<MemberAccessExpressionSyntax>()
            .Any(memberAccess => memberAccess.Name.ToString().ToLower() == methodName.ToLower());
    }

    static bool IsMarkedAsNullable(PropertyInfo p)
    {
        return new NullabilityInfoContext().Create(p).WriteState is NullabilityState.Nullable;
    }
}