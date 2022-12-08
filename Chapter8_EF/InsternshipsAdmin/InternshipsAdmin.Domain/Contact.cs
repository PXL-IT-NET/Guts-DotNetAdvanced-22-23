using InsternshipsAdmin.UI;

namespace InternshipsAdmin.Domain
{
    public class Contact:Person
    {
        public Contact(string name):base(name)
        {

        }
        public Contact(String name, Company company):base(name)
        {           
            Company = company;
        }
        public string? Prefix { get; set; }

        public Company Company { get; set; }
        public int CompanyId { get; set; }
    }
}
