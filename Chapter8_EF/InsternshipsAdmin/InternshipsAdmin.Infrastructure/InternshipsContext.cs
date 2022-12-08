using InternshipsAdmin.Domain;
using Microsoft.EntityFrameworkCore;

namespace InternshipsAdmin.Infrastructure
{
    public class InternshipsContext: DbContext
    {
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<Supervisor> Supervisors => Set<Supervisor>();

        public InternshipsContext() { }

        public InternshipsContext(DbContextOptions<InternshipsContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) //only configure the connection if the parameterless contructor was used (no options where provided).
            {
                string connectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=Internships";
                    //ConfigurationManager.ConnectionStrings["InternshipsConnectionString"].ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            Company company1 = new Company("PXL Smart ICT", "Elfde Liniestraat 24", "3500", "Hasselt");
            company1.CompanyId = 1;
            modelBuilder.Entity<Company>().HasData(company1);

            Company company2 = new Company("Datasense", "Kempische Steenweg 309 Bus 1.01", "3500", "Hasselt");
            company2.CompanyId = 2;
            modelBuilder.Entity<Company>().HasData(company2);           
        }
    }
}
