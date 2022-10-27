using LinqExamples.Models;

namespace LinqExamples.Tests;

[TestFixture]
public class WhereExamplesTests
{
    private WhereExamples _examples = null!;

    [SetUp]
    public void Setup()
    {
        _examples = new WhereExamples();
    }

    [Test] public void FilterOutNumbersDivisibleByTen_ShouldOnlyReturnNumbersDivisibleByTen()
    {
        //Arrange
        int[] numbers = { 10, 11, 15, 20, 100, 101 };
        int[] expected = { 10, 20, 100 };

        //Act
        var actual = _examples.FilterOutNumbersDivisibleByTen(numbers);

        //Assert
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void PersonsThatAreEighteenOrOlderCanBeFilteredOutUsingWhere()
    {
        //Arrange
        var persons = new List<Person>
        {
            new() {Name = "John", BirthDate = DateTime.Now.AddYears(-11)},
            new() {Name = "Jane", BirthDate = DateTime.Now.AddYears(-54)},
            new() {Name = "Jules", BirthDate = DateTime.Now.AddYears(-17)},
            new() {Name = "Jeffry", BirthDate = DateTime.Now.AddYears(-20)},
            new() {Name = "Joe", BirthDate = DateTime.Now.AddYears(-15)}
        };

        //Act
        var adults = _examples.FilterOutPersonsThatAreEighteenOrOlder(persons);

        //Assert
        Assert.That(adults, Has.All.Matches((Person p) => p.Name == "Jane" || p.Name == "Jeffry"));
    }
}