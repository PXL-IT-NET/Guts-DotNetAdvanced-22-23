using InternshipsAdmin.Domain;
using InternshipsAdmin.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace InternshipsAdmin.Tests
{
    public class InternshipsTestContext:InternshipsContext
    {
        public InternshipsTestContext() { }

        public InternshipsTestContext(DbContextOptions<InternshipsTestContext> options) { }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) //only configure the connection if the parameterless contructor was used (no options where provided).
            {
                string connectionString = "Data Source=(localdb)\\mssqllocaldb; Initial Catalog=InternshipsTest";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //needed to test string type in In-memory Sqlite database
            modelBuilder.Entity<Company>().Property(p => p.Name).HasColumnType("text");
            modelBuilder.Entity<Company>().Property(p => p.Address).HasColumnType("text");
            modelBuilder.Entity<Company>().Property(p => p.Zip).HasColumnType("text");
            modelBuilder.Entity<Company>().Property(p => p.City).HasColumnType("text");

            modelBuilder.Entity<Contact>().Property(p => p.Prefix).HasColumnType("text");

            modelBuilder.Entity<Contact>().Property(p => p.Name).HasColumnType("text");
            modelBuilder.Entity<Contact>().Property(p => p.Email).HasColumnType("text");
            modelBuilder.Entity<Contact>().Property(p => p.Phone).HasColumnType("text");


            modelBuilder.Entity<Student>().Property(p => p.Name).HasColumnType("text");
            modelBuilder.Entity<Student>().Property(p => p.Email).HasColumnType("text");
            modelBuilder.Entity<Student>().Property(p => p.Phone).HasColumnType("text");

            modelBuilder.Entity<Supervisor>().Property(p => p.Name).HasColumnType("text");
            modelBuilder.Entity<Supervisor>().Property(p => p.Email).HasColumnType("text");
            modelBuilder.Entity<Supervisor>().Property(p => p.Phone).HasColumnType("text");

            modelBuilder.Entity<Student>().Property(p => p.Department).HasColumnType("text");

            modelBuilder.Entity<Supervisor>().Property(p => p.JobTitle).HasColumnType("text");
            modelBuilder.Entity<Supervisor>().Property(p => p.JobTitle).HasColumnType("text");
            modelBuilder.Entity<Supervisor>().Property(p => p.Specialism).HasColumnType("text");
        }
    }
}
