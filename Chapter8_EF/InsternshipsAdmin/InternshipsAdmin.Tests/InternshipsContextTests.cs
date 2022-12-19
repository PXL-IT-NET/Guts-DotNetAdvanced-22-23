using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using InternshipsAdmin.Domain;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace InternshipsAdmin.Tests
{
    [ExerciseTestFixture("dotnet2", "H08", "Exercise01", @"InternshipsAdmin.Infrastructure\InternshipsContext.cs")]
    internal class InternshipsContextTests : DatabaseTests
    {
        private string _internshipsContextClassContent;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _internshipsContextClassContent = Solution.Current.GetFileContent(@"InternshipsAdmin.Infrastructure\InternshipsContext.cs");
        }

        [MonitoredTest("InternshipsContext - Should have four DBSets")]
        public void ShouldHaveFourDbSets()
        {
            var properties = GetDbSetProperties();

            Assert.That(properties, Has.Count.EqualTo(4), () => "There should be exactly 4 'DbSet' properties.");
            Assert.That(properties,
                Has.One.Matches((PropertyDeclarationSyntax p) => p.Type.ToString() == "DbSet<Company>"),
                () => "Ther should be one 'DbSet' for Companies.");
            Assert.That(properties,
                Has.One.Matches((PropertyDeclarationSyntax p) => p.Type.ToString() == "DbSet<Student>"),
                () => "Ther should be one 'DbSet' for students.");
            Assert.That(properties,
                Has.One.Matches((PropertyDeclarationSyntax p) => p.Type.ToString() == "DbSet<Contact>"),
             () => "Ther should be one 'DbSet' for contacts.");
            Assert.That(properties,
                Has.One.Matches((PropertyDeclarationSyntax p) => p.Type.ToString() == "DbSet<Supervisor>"),
                () => "Ther should be one 'DbSet' for supervisors.");
        }

        [MonitoredTest("InternshipsContext - OnModelCreating - Should seed at least 2 companies")]
        public void OnModelCreating_ShouldSeedAtLeast2Companies()
        {
            using (var context = CreateDbContext())
            {
                var amountOfSeededCompanies = context.Set<Company>().Count();
                Assert.That(amountOfSeededCompanies, Is.GreaterThanOrEqualTo(2), () => "The database must be seeded wit at least 2 companies. " +
                                                                                   $"Now the database contains {amountOfSeededCompanies} companies after creation.");
            }
        }

        [MonitoredTest("InternshipsContext - OnConfiguring - Should configure a Sql Database")]
        public void OnConfiguring_ShouldConfigureASqlDatabaseConnection()
        {
            var methodBody = GetMethodBody("OnConfiguring");

            Assert.That(methodBody.Statements.Count == 1 && methodBody.Statements[0] is IfStatementSyntax, Is.True,
                () => "The method body should only have one if-statement. " +
                      "The body of the if-statement should contain the code to configure the database.");

            var ifStatement = (IfStatementSyntax)methodBody.Statements[0];
            Assert.That(ifStatement.Else, Is.Null, () => "The if-statement does not need to have an 'else'.");
            var ifStatementBody = ifStatement.ToString();

            Assert.That(ifStatementBody,
                Contains.Substring("\"Data Source=(localdb)\\\\mssqllocaldb; Initial Catalog=Internships\""),
                () => "The connectionstring should be hard coded in the OnConfiguring method.");

            Assert.That(ifStatementBody, Contains.Substring("optionsBuilder.UseSqlServer("),
                () => "You should tell Entity Framework that is should use the SQL Server provider.");
        }

        private BlockSyntax GetMethodBody(string methodName)
        {
            var syntaxtTree = CSharpSyntaxTree.ParseText(_internshipsContextClassContent);
            var root = syntaxtTree.GetRoot();
            var method = root
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(md => md.Identifier.ValueText.Equals(methodName));
            Assert.That(method, Is.Not.Null,
                () => $"Could not find the '{methodName}' method. You may have accidentially deleted or renamed it?");
            return method.Body;
        }

        private IList<PropertyDeclarationSyntax> GetDbSetProperties()
        {
            var syntaxtTree = CSharpSyntaxTree.ParseText(_internshipsContextClassContent);
            var root = syntaxtTree.GetRoot();
            var properties = root
                .DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .Where(p =>
                {
                    if (!(p.Type is GenericNameSyntax genericName)) return false;
                    return genericName.Identifier.ValueText == "DbSet";
                });

            return properties.ToList();
        }
    }
}
