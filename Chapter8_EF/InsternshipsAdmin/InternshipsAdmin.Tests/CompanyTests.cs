using Guts.Client.Core;
using InternshipsAdmin.Domain;
using System.Collections;

namespace InternshipsAdmin.Tests
{
    [ExerciseTestFixture("dotnet2", "H08", "Exercise01",
    @"InternshipsAdmin.Domain\Company.cs")]
    internal class CompanyTests : TestBase
    {
        private Type _companyType = null!;
        private Type _contactType = null!;

        [SetUp]
        public void Setup()
        {
            _companyType = typeof(Company);
            _contactType = typeof(Contact);
        }

        [MonitoredTest("Company - Should have a One-to-One relation with Contact")]
        public void _01_Company_ShouldHaveAOneToOneRelationWithContact()
        {   
            AssertHasPublicPropertyOfType(_companyType, nameof(Company.Contact), typeof(Contact));
            AssertHasPublicPropertyOfType(_contactType, nameof(Contact.Company), typeof(Company));
            AssertHasPublicPropertyOfType(_contactType, nameof(Company.CompanyId), typeof(int));
        }

        [MonitoredTest("Company - Should have a One-to-Many relation with Supervisor")]
        public void _02_Company_ShouldHaveAOneToManyRelationWithSupervisor()
        {
            var type = nameof(Company.Supervisors).GetType();
            AssertHasPublicProperty(_companyType, nameof(Company.Supervisors));
            Assert.That(typeof(IEnumerable).IsAssignableFrom(type), Is.True);
        }
    }
}
