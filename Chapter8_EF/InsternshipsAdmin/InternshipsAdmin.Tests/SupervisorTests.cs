using Guts.Client.Core;
using InternshipsAdmin.Domain;
using System.Collections;

namespace InternshipsAdmin.Tests
{
    [ExerciseTestFixture("dotnet2", "H08", "Exercise01", @"InternshipsAdmin.Domain\Supervisor.cs")]
    internal class DomainTests : TestBase
    {
        private Type _companyType = null!;
        private Type _supervisorType = null!;
        private Type _contactType = null!;

        [SetUp]
        public void Setup()
        {
            _supervisorType = typeof(Supervisor);
        }

        [Test]
        public void _03_Supervisor_ShouldHaveAOneToManyRelationWithStudent()
        {
            var type = nameof(Supervisor.Students).GetType();
            AssertHasPublicProperty(_supervisorType, nameof(Supervisor.Students));
            Assert.That(typeof(IEnumerable).IsAssignableFrom(type), Is.True);
        }

    }
}
