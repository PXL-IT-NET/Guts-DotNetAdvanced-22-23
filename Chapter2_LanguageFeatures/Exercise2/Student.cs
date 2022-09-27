namespace Exercise2
{
    public class Student
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string? Department { get; }

        public Student(string firstName, string lastName, string? department)
        {
            FirstName = firstName;
            LastName = lastName;
            Department = department;
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName} - {Department ?? "/"}";
        }
    }
}

