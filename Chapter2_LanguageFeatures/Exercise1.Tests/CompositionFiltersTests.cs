using System.Reflection;

namespace Exercise1.Tests;

[ExerciseTestFixture("dotNet2", "H02", "Exercise01", @"Exercise1\CompositionFilters.cs")]
public class CompositionFiltersTests
{
    [MonitoredTest("CompositionFilters - Should have 3 Public Static Methods of ReturnType CompositionFilterDelegate Properties")]
    public void _01_ShouldHave3PublicStaticMethodsOfReturnTypeCompositionFilterDelegate()
    {
        Type classType = typeof(CompositionFilters);
        MethodInfo[] compositionFiltersMethods = classType.GetMethods(BindingFlags.Public | BindingFlags.Static);
        Assert.That(compositionFiltersMethods.Length, Is.EqualTo(3),
            () => "Class CompositionFilters should have 3 static methods");
        foreach (var method in compositionFiltersMethods)
        {
            var parameter = method.ReturnParameter;
            Assert.That(parameter.ParameterType, Is.EqualTo(typeof(CompositionFilterDelegate)),
                "Returntype of the methods should be 'CompositionFilterDelegate'");
        }
    }

    [MonitoredTest("CompositionFilters - QuickFilter - Should search Keyword in Title")]
    public void _02_QuickFilter_ShouldSearchKeywordInTitle()
    {
        Composition composition = new Composition
        {
            Title = Guid.NewGuid().ToString()
        };
        string searchKey = composition.Title.Substring(0, 5);
        Assert.That(CompositionFilters.QuickFilter(composition, searchKey), Is.True,
            "The QuickFilter method should search the searchkey in the Title of the composition");
        Assert.That(CompositionFilters.QuickFilter(composition, Guid.NewGuid().ToString()), Is.False,
            "The QuickFilter method should return false when the searchkey is not in the Title of the composition");
    }

    [MonitoredTest("CompositionFilters - DetailedFilter - Should search Keyword in Title and Description")]
    public void _03_DetailedFilter_ShouldSearchKeywordInTitleAndDescription()
    {
        Composition composition = new Composition
        {
            Title = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString()
        };
        string searchKeyInTitle = composition.Title.Substring(0, 5);
        string searchKeyInDescription = composition.Description.Substring(0, 5);
        string nonMatchingSearchKey = Guid.NewGuid().ToString();
        Assert.That(CompositionFilters.DetailedFilter(composition, searchKeyInTitle), Is.True,
            "The DetailedFilter method should search the searchkey in the Title and the Description of the composition");
        Assert.That(CompositionFilters.DetailedFilter(composition, searchKeyInDescription), Is.True,
            "The DetailedFilter method should search the searchkey in the Title and the Description of the composition");
        Assert.That(CompositionFilters.DetailedFilter(composition, nonMatchingSearchKey), Is.False,
            "The DetailedFilter method should return false when the searchkey is not in the Title or the Description of the composition");
    }

    [MonitoredTest("CompositionFilters - ReleaseYearFilter - Should search year in ReleaseYear")]
    public void _03_ReleaseYearFilter_ShouldSearchYearInReleaseYear()
    {
        Composition composition = new Composition();
        DateTime date = DateTime.Now;
        DateTime randomDate = date.Next();

        composition.ReleaseDate = randomDate;
        Assert.That(CompositionFilters.ReleaseYearFilter(composition, randomDate.Year.ToString()), Is.True,
            "The ReleaseYearFilter method should search for a Composition with the passed ReleaseYear");

        Assert.That(CompositionFilters.ReleaseYearFilter(composition, $"{randomDate.Year + 1}"), Is.False,
            "The ReleaseYearFilter method should return false when the searchKey is a different year than the ReleaseYear of the composition");
    }
}