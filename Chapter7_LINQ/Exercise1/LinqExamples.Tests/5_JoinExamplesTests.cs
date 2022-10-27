using LinqExamples.Models;

namespace LinqExamples.Tests;

[TestFixture]
public class JoinExamplesTests
{
    private JoinExamples _examples = null!;

    [SetUp]
    public void Setup()
    {
        _examples = new JoinExamples();
    }

    [Test] public void GetEvenNumbersOfIntersection_ShouldReturnEvenNumbersThatAreInBothLists()
    {
        //Arrange
        int[] firstList = { 2, 3, 5, 10 };
        int[] secondList = { 3, 8, 2, 10, 1 };
        int[] expected = { 2, 10 };

        //Act
        var intersection = _examples.GetEvenNumbersOfIntersection(firstList, secondList);

        //Assert
        Assert.That(intersection, Has.All.Matches((int n) => expected.Contains(n)));
    }

    [Test]
    public void MatchPersonsOnBirthMonth_ShouldReturnCouplesThatAreBornInTheSameMonth()
    {
        //Arrange
        var birthYearDate = DateTime.Today.AddYears(-10);
        var group1 = new List<Person>
        {
            new() {Name = "John", BirthDate = birthYearDate.AddMonths(-4)},
            new() {Name = "Jules", BirthDate = birthYearDate.AddMonths(-8)},
            new() {Name = "Jeffry", BirthDate = birthYearDate.AddMonths(-5 - 12)},
            new() {Name = "Jay", BirthDate = birthYearDate.AddMonths(-1)},
        };

        var group2 = new List<Person>
        {
            new() {Name = "Jane", BirthDate = birthYearDate.AddMonths(-11)},
            new() {Name = "Jennifer", BirthDate = birthYearDate.AddMonths(-1 - (2 * 12))},
            new() {Name = "Joan", BirthDate = birthYearDate.AddMonths(-5 - (3 * 12))},
            new() {Name = "Jill", BirthDate = birthYearDate.AddMonths(-5)},
        };

        var expected = new List<string>
        {
            "Jeffry and Joan", "Jeffry and Jill", "Jay and Jennifer"
        };

        //Act
        IList<string> couples = _examples.MatchPersonsOnBirthMonth(group1, group2);

        //Assert
        Assert.That(couples.Count, Is.EqualTo(expected.Count));
        Assert.That(couples, Has.All.Matches((string couple) => expected.Contains(couple)));
    }
}