using System.Reflection;

namespace Exercise1.Tests;

[ExerciseTestFixture("dotNet2", "H02", "Exercise01", @"Exercise1\CompositionSearcher.cs")]
public class CompositionSearcherTests
{

    [MonitoredTest("CompositionSearcher - Should have one Private field of type IList<Composer>")]
    public void _01_ShouldHaveOnePrivateFieldOfTypeIListOfCompositions()
    {
        Type type = typeof(CompositionSearcher);
        FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

        Assert.That(fields.Length, Is.GreaterThan(0), "The Compostion Class should have one private field");

        Type typeOfField = fields[0].FieldType;

        var typeList = typeof(IList<Composition>);

        Assert.That(typeOfField, Is.EqualTo(typeList), "The Composition Class should have one private field of type List<Composition>");
    }

    [MonitoredTest("CompositionSearcher - Should have constructor that fills the private field with compositions")]
    public void _02_ShouldHaveConstructorWhoFillsThatFillsThePrivateFieldWithCompositions()
    {
        CompositionSearcher searcher = new CompositionSearcher();
        Assert.That(typeof(CompositionSearcher).HasDefaultConstructor(), Is.True,  "CompositionSearcher Class should have a parameterless default Constructor");
        FieldInfo field = typeof(CompositionSearcher).GetField("_allCompositions", BindingFlags.NonPublic | BindingFlags.Instance);
        List<Composition> compositions = (List<Composition>)field.GetValue(searcher);
        Assert.That(compositions.Count, Is.EqualTo(4), "Constructor should call GetCompositions to fill the private member field");
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