using InsternshipsAdmin.UI;

namespace InternshipsAdmin.Domain
{
    public class Student:Person
    {
        public Student(string name):base(name)
        {
        }
        public string? Department { get; set; }

        public int? SupervisorId { get; set; }
        public Supervisor? Supervisor { get; set; }

        
    }
}
