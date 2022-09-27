using Guts.Client.Core;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Reflection;
using Guts.Client.Core.TestTools;

namespace Exercise1.Tests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class CompositionTests
    {
        private string _compositionClassContent;

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            _compositionClassContent = Solution.Current.GetFileContent(@"Exercise1\Composition.cs");
        }

        [MonitoredTest("Should have all Properties")]
        public void _01_ShouldHaveAllProperties()
        {
            Assert.That(typeof(Composition).HasProperty("Title"), Is.True, () => "Title Property is missing in the Composition class");
            Assert.That(typeof(Composition).HasProperty("Description"), Is.True, () => "Description Property is missing in the Composition class");
            Assert.That(typeof(Composition).HasProperty("Composer"), Is.True, () => "Composer Property is missing in the Composition class");
            Assert.That(typeof(Composition).HasProperty("ReleaseDate"), Is.True, () => "ReleaseDate Property is missing in the Composition class");
        }

        [MonitoredTest("Should have Nullable Composer Property")]
        public void _02_ShouldHaveNullableComposerProperty()
        {
            var props = typeof(Composition).GetProperties();
            foreach(var prop in props)
            {
                if(prop.Name=="Composer")
                {
                    Assert.That(IsMarkedAsNullable(prop), Is.True, () => "Composer property has to be nullable"); 
                }
            }
        }

        [MonitoredTest("Should have parameterless Constructor")]
        public void _03_ShouldHaveParameterLessConstructor()
        {
            Assert.That(typeof(Composition).HasDefaultConstructor(), Is.True, "Composite class must have a parameterless constructor");
        }


        [MonitoredTest("Constructor Should assign an Empty String to Title and description")]
        public void _04_ConstructorShouldAssignEmptyStringToTitleAndDescriptionProperty()
        {
            var composition = new Composition();
            Assert.That(composition.Title, Is.Empty, "CompositeCompotion constructor should assign an empty string to Title");
            Assert.That(composition.Description, Is.Empty, "CompositeCompotion constructor should assign an empty string to Description");
        }

        [MonitoredTest("Constructor Should override ToString Method")]
        public void _05_ConstructorShouldOverrideToStringMethod()
        {
            Composition composition = new Composition();
            Type objType = typeof(Composition);
            MethodInfo info = objType.GetMethod("ToString");
            Assert.That(info, Is.Not.Null, () => "Composition Constructor should contain a ToString Method");
            Assert.That(objType.IsSubclassOf(info.DeclaringType), Is.False, () => "ToString method is not overridden");
            Assert.That(info.IsVirtual, Is.True, "ToString method should override de ToString base method");
        }


        [MonitoredTest("ToString Should use the ToCentury Extension method")]
        public void _06_ToStringShouldUseToCenturyExtensionMethod()
        {
            Composition comp = new Composition();
            comp.Description = Guid.NewGuid().ToString();
            comp.Title = Guid.NewGuid().ToString();
            comp.ReleaseDate = DateTime.Today;
            comp.ToString(); //check if the program runs without exceptions

            Assert.That(CallsMemberMethod("ToCentury"), Is.True, "Cannot find an invocation of the 'ToCentury' method of a 'Composition' instance.");
        }

        private bool CallsMemberMethod(string methodName)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(_compositionClassContent);
            var root = syntaxTree.GetRoot();
            return root
                .DescendantNodes()
                .OfType<MemberAccessExpressionSyntax>()
                .Any(memberAccess => memberAccess.Name.ToString().ToLower() == methodName.ToLower());
        }

        static bool IsMarkedAsNullable(PropertyInfo p)
        {
            return new NullabilityInfoContext().Create(p).WriteState is NullabilityState.Nullable;
        }
    }
}
