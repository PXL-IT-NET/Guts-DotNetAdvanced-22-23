using LinqExamples.Models;

namespace LinqExamples.Tests;

[TestFixture]
public class OrderByExamplesTests
{
    private OrderByExamples _examples = null!;

    [SetUp]
    public void Setup()
    {
        _examples = new OrderByExamples();
    }

    [Test]
    public void SortAnglesFromBigToSmall_ShouldReturnSortedAngles()
    {
        //Arrange
        double[] angles = { 10d, 350d, 45d, 90d };
        double[] expected = { 350d, 90d, 45d, 10d };

        //Act
        var actual = _examples.SortAnglesFromBigToSmall(angles);

        //Assert
        Assert.That(actual, Is.EquivalentTo(expected));
    }

    [Test]
    public void SortPersonsFromYoungToOldAndThenOnNameAlphabetically_ShouldReturnSortedPersons()
    {
        //Arrange
        DateTime today = DateTime.Today;
        Person joe = new Person{Name = "Joe", BirthDate = today.AddYears(-20)};
        Person john = new Person{Name = "John", BirthDate = today.AddYears(-20)};
        Person jane = new Person{Name = "Jane", BirthDate = today.AddYears(-54)};
        Person jules = new Person{Name = "Jules", BirthDate = today.AddYears(-17)};
        Person jeffrey = new Person{Name = "Jeffry", BirthDate = today.AddYears(-20)};

        var persons = new List<Person>
        {
            joe,
            john,
            jane,
            jules,
            jeffrey
        };

        var expected = new List<Person>
        {
            jules,
            joe,
            jeffrey,
            john,
            jane
        };

        //Act
        var actual = _examples.SortPersonsFromYoungToOldAndThenOnNameAlphabetically(persons);

        //Assert
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}