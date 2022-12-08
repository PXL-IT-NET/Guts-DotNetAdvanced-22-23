namespace InternshipsAdmin.Domain
{
    public class Company
    {   
        public Company() { Supervisors = new List<Supervisor>(); }
        public Company(string name, string address, string zip, string city)
        {
            Supervisors = new List<Supervisor>();
            Name = name;
            Address = address;
            Zip = zip;
            City = city;            
        }
        public int CompanyId { get; set; }
        
        public string Name { get; set; }
       
        public string Address { get; set; }
       
        public string Zip { get; set; }
        
        public string City { get; set; }

        public Contact? Contact { get; set; }
        public List<Supervisor>? Supervisors { get; set; }

    }
}