using InsternshipsAdmin.UI;

namespace InternshipsAdmin.Domain
{
    public class Supervisor:Person
    {
        public Supervisor(string name):base(name)
        {
            Students=new List<Student>();
        }

        public string? JobTitle { get; set; }

        public string? Specialism { get; set; }

        public List<Student>? Students { get; set; }

        public Company Company { get; set; }
        public int CompanyId { get; set; }

    }
}
