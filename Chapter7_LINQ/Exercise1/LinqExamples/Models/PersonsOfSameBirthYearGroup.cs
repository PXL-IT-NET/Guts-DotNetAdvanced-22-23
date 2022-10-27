namespace LinqExamples.Models;

public class PersonsOfSameBirthYearGroup
{
    public int BirthYear { get; set; }
    public IEnumerable<Person> Persons { get; set; }

    public PersonsOfSameBirthYearGroup()
    {
        BirthYear = 0;
        Persons = new List<Person>();
    }
}