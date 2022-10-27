using LinqExamples.Models;

namespace LinqExamples.Tests;

[TestFixture]
public class GroupExamplesTests
{
    private GroupExamples _examples = null!;

    [SetUp]
    public void Setup()
    {
        _examples = new GroupExamples();
    }

    [Test] public void GroupEvenAndOddNumbers_ShouldReturn2GroupsOfNumbers_OneGroupEven_OneGroupOdd()
    {
        //Arrange
        int[] numbers = { 5, 7, 15, 18, 25, 28 };
        int[] expectedOddNumbers = { 5, 7, 15, 25 };
        int[] expectedEvenNumbers = { 18, 28 };

        //Act
        IList<IGrouping<int, int>> results = _examples.GroupEvenAndOddNumbers(numbers);

        //Assert
        Assert.That(results.Count, Is.EqualTo(2));
        var oddNumbers = results[0];
        Assert.That(oddNumbers, Is.EquivalentTo(expectedOddNumbers));
        var evenNumbers = results[1];
        Assert.That(evenNumbers, Is.EquivalentTo(expectedEvenNumbers));
    }

    [Test]
    public void GroupPersonsByBirthYear_ShouldReturnPersonsOfSameBirthYearGroups()
    {
        //Arrange
        var today = DateTime.Today;
        var persons = new List<Person>
        {
            new() {Name = "John", BirthDate = today.AddYears(-20)},
            new() {Name = "Jane", BirthDate = today.AddYears(-30)},
            new() {Name = "Jules", BirthDate = today.AddYears(-30)},
            new() {Name = "Jeffry", BirthDate = today.AddYears(-20)},
            new() {Name = "Joe", BirthDate = today.AddYears(-30)}
        };

        //Act
        IList<PersonsOfSameBirthYearGroup> personAgeGroups = _examples.GroupPersonsByBirthYear(persons);

        //Assert
        Assert.That(personAgeGroups.Count, Is.EqualTo(2));

        PersonsOfSameBirthYearGroup? twentiers = personAgeGroups.FirstOrDefault(group => group.BirthYear == today.AddYears(-20).Year);
        Assert.That(twentiers, Is.Not.Null);
        Assert.That(twentiers.Persons.Count, Is.EqualTo(2));
        Assert.That(twentiers.Persons, Has.All.Matches((Person p) => p.BirthDate.Year == today.AddYears(-20).Year));

        var thirtiers = personAgeGroups.FirstOrDefault(group => group.BirthYear == today.AddYears(-30).Year);
        Assert.That(thirtiers, Is.Not.Null);
        Assert.That(thirtiers.Persons.Count, Is.EqualTo(3));
        Assert.That(thirtiers.Persons, Has.All.Matches((Person p) => p.BirthDate.Year == today.AddYears(-30).Year));
    }
}