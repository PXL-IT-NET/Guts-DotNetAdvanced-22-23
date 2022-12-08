namespace InsternshipsAdmin.UI
{
    public class Person
    {
        public Person(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name is required.", nameof(name));
            }
            Name = name;
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
