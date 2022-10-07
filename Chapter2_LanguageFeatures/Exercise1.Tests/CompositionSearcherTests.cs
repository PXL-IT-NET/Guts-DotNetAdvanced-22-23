using System.Reflection;

namespace Exercise1.Tests;

[ExerciseTestFixture("dotNet2", "H02", "Exercise01", @"Exercise1\CompositionSearcher.cs")]
public class CompositionSearcherTests
{

    [MonitoredTest("CompositionSearcher - Should have one Private field of type IList<Composer>")]
    public void _01_ShouldHaveOnePrivateFieldOfTypeIListOfCompositions()
    {
        GetCompositionListField();
    }

    private FieldInfo GetCompositionListField()
    {
        FieldInfo[] allFields = typeof(CompositionSearcher).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.That(allFields.Length, Is.GreaterThan(0), "Cannot find any private fields in the class");
        FieldInfo? field = allFields.FirstOrDefault(fi => fi.FieldType.IsAssignableTo(typeof(IList<Composition>)));
        Assert.That(field, Is.Not.Null, "The class should have one private field of type IList<Composition>");
        return field!;
    }

    [MonitoredTest("CompositionSearcher - Should have constructor that fills the private field with compositions")]
    public void _02_ShouldHaveConstructorWhoFillsThatFillsThePrivateFieldWithCompositions()
    {
        CompositionSearcher searcher = new CompositionSearcher();
        FieldInfo field = GetCompositionListField();

        IList<Composition>? compositions = field.GetValue(searcher) as IList<Composition>;
        Assert.That(compositions, Is.Not.Null, "The private field that contains the compositions should not be null");
        Assert.That(compositions.Count, Is.EqualTo(4), "Constructor should call GetCompositions to fill the private field");
    }

    [MonitoredTest("CompositionSearcher - SearchMusic - Should use delegate to filter compositions")]
    public void _03_SearchMusic_ShouldUseDelegateToFilterCompositions()
    {
        CompositionSearcher searcher = new CompositionSearcher();

        string keyword = Guid.NewGuid().ToString();
        int keywordUsages = 0;
        CompositionFilterDelegate filter = (composition, searchKeyword) =>
        {
            if (searchKeyword == keyword)
            {
                keywordUsages++;
            }
            return keywordUsages % 2 == 0;
        };
        var results = searcher.SearchMusic(filter, keyword);
        Assert.That(keywordUsages, Is.EqualTo(4), "The delegate should be invoked exactly 4 times (there are 4 compositions).");
        Assert.That(results.Count, Is.EqualTo(2),
            "The delegate should be used to filter compositions. " +
            "The delegate for this test should result in exactly 2 compositions being returned.");
    }
}