using Guts.Client.Core;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Reflection;

namespace Exercise1.Tests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class CompositionFilterDelegateTests
    {       
        private TypeInfo _compositionFilterDelegateTypeInfo;

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            var assembly = Assembly.GetAssembly(typeof(Program));
            _compositionFilterDelegateTypeInfo = assembly.DefinedTypes.FirstOrDefault(t =>
            {
                if (!typeof(MulticastDelegate).IsAssignableFrom(t)) return false;

                //check signature (must return bool and have a Composition and a string parameter)
                var methodInfo = t.DeclaredMethods.First(p => p.Name == "Invoke");
               
                if (methodInfo.ReturnType.Name.ToLower() != "boolean") return false;
                ParameterInfo[] parameters = methodInfo.GetParameters();
                if (parameters.Length != 2) return false;
                if (parameters[0].ParameterType != typeof(Composition)) return false;
                if (parameters[1].ParameterType != typeof(string)) return false;
                return true;
            });
        }

        [MonitoredTest("There Should be a compositionFilterDelegate Type defined"), Order(1)]
        public void _01_ThereShouldBeACompositionFilterDelegateTypeDefined()
        {
            AssertCompositionFilterDelegateIsDefinedCorrectly();
        }

        private void AssertCompositionFilterDelegateIsDefinedCorrectly()
        {
            Assert.IsNotNull(_compositionFilterDelegateTypeInfo,
                "Cannot find a delegate type definition that supports methods that return void and accept a string parameter. " +
                "Define the type in 'WriteDelegate.cs'.");
        }
    }
}