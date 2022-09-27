using Guts.Client.Core;
using System.Reflection;

namespace Exercise1.Tests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class CompositionFiltersTests
    {
        [MonitoredTest("Should have 3 Public Static Methods of ReturnType CompositionFilterDelegate Properties")]
        public void _01_ShouldHave3PublicStaticMethodsOfReturnTypeCompositionFilterDelegate()
        {
            Type classType = typeof(CompositionFilters);
            MethodInfo[] compositionFiltersMethods = classType.GetMethods(BindingFlags.Public | BindingFlags.Static);
            Assert.That(compositionFiltersMethods.Length, Is.EqualTo(3), () => "Class CompositionFilters should have 3 static methods");
            foreach(var method in compositionFiltersMethods)
            {
                var parameter = method.ReturnParameter;
                Assert.That(parameter.ParameterType, Is.EqualTo(typeof(CompositionFilterDelegate)),() => "Returntype of the methods should be 'CompositionFilterDelegate'");
            }
        }

        [MonitoredTest("QuicFilter method Should search Keyword in Title")]
        public void _02_QuickFilterMethodShouldSearchKeywordInTitle()
        {
            Composition comp = new Composition();
            comp.Title = Guid.NewGuid().ToString();
            string searchKey = comp.Title.Substring(0, 5);
            Assert.That(CompositionFilters.QuickFilter(comp, searchKey), Is.True, ()=>"The QuickFilter method should search the searchkey in the Title of the composition");
        }

        [MonitoredTest("DetailedFilter method Should search Keyword in Title and Description")]
        public void _03_DetailedFilterMethodShouldSearchKeywordInTitleAndDescription()
        {
            Composition comp = new Composition();
            comp.Title = Guid.NewGuid().ToString();
            comp.Description= Guid.NewGuid().ToString();
            string searchKeyInTitle = comp.Title.Substring(0, 5);
            string searchKeyInDescription = comp.Description.Substring(0, 5);
            Assert.That(CompositionFilters.DetailedFilter(comp, searchKeyInTitle), Is.True, () => "The DetailedFilter method should search the searchkey in the Title and the Description of the composition");
            Assert.That(CompositionFilters.DetailedFilter(comp, searchKeyInDescription), Is.True, () => "The DetailedFilter method should search the searchkey in the Title and the Description of the composition");
        }

        [MonitoredTest("ReleasYearFilter method Should search year in ReleaseYear")]
        public void _03_ReleaseYearFilterMethodShouldSearchYearInReleaseYear()
        {
            Composition comp = new Composition();
            DateTime date = DateTime.Now;
            DateTime randomDate = date.Next();
            
            comp.ReleaseDate = randomDate;
            Assert.That(CompositionFilters.ReleaseYearFilter(comp, randomDate.Year.ToString()), Is.True, () => "The ReleaseYearFilter method should search for a Composition with the passed ReleaseYear");
        }
    }
}
