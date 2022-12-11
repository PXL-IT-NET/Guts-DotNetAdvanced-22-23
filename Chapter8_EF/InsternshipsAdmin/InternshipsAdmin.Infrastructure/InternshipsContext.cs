using InternshipsAdmin.Domain;
using Microsoft.EntityFrameworkCore;

namespace InternshipsAdmin.Infrastructure
{
    public class InternshipsContext: DbContext
    {
        /*
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<Supervisor> Supervisors => Set<Supervisor>();
        */

        public InternshipsContext() { }

        public InternshipsContext(DbContextOptions<InternshipsContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            throw new NotImplementedException();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();

        }
    }
}
