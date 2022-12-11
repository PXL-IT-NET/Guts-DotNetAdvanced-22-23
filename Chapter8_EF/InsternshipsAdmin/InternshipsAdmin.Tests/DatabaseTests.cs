using InternshipsAdmin.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace InternshipsAdmin.Tests
{
    internal abstract class DatabaseTests : IDisposable
    {
        private SqliteConnection _connection;
        private string _migrationError;

        public DatabaseTests()
        {
            _migrationError = string.Empty;
        }

        [OneTimeSetUp]
        public void CreateDatabase()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            using (var context = CreateDbContext(false))
            {             

                //Check if migration succeeded
                try
                {
                    context.Database.Migrate();
                    context.Find<Company>(1);
                    context.Find<Student>(1);
                    context.Find<Supervisor>(1);
                    context.Find<Contact>(1);
                }
                catch (Exception e)
                {
                    var messageBuilder = new StringBuilder();
                    messageBuilder.AppendLine("The migration (creation) of the database is not configured properly.");
                    messageBuilder.AppendLine();
                    messageBuilder.AppendLine(e.Message);
                    _migrationError = messageBuilder.ToString();
                }
            }
        }

        [OneTimeTearDown]
        public void DropDatabase()
        {
            using (var context = CreateDbContext(false))
            {
                context.Database.EnsureDeleted();
            }
            _connection?.Close();
        }

        protected InternshipsTestContext CreateDbContext(bool assertMigration = true)
        {
            if (assertMigration)
            {
                AssertMigratedSuccessfully();
            }

            var options = new DbContextOptionsBuilder<InternshipsTestContext>()
                .UseSqlite(_connection)
                .Options;

            return new InternshipsTestContext(options);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        private void AssertMigratedSuccessfully()
        {
            if (!string.IsNullOrEmpty(_migrationError))
            {
                Assert.Fail(_migrationError);
            }
        }
    }
}