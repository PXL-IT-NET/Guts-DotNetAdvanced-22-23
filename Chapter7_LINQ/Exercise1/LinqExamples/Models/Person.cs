namespace LinqExamples.Models;

public class Person
{
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }

    public Person()
    {
        Name = string.Empty;
        BirthDate = DateTime.MinValue;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Person other)) return false;

        return other.Name.Equals(Name) && other.BirthDate.Equals(BirthDate);
    }
}