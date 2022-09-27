using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace Exercise1.Tests
{
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class CompositionSearcherTests
    {
        private string _compositionSearcherClassContent;
        private TypeDelegator _compositionSearcherTypeInfo;

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            _compositionSearcherTypeInfo = new TypeDelegator(typeof(CompositionSearcher));
            _compositionSearcherClassContent = Solution.Current.GetFileContent(@"Exercise1\CompositionSearcher.cs");
        }

        [MonitoredTest("Should have one Private field of type IList<Composer>")]
        public void _01_ShouldHaveOnePrivateFieldOfTypeIListOfCompositions()
        {
            Type type = typeof(CompositionSearcher);
            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.That(fields.Length, Is.GreaterThan(0), () => "The Compostion Class should have one private field");

            Type typeOfField = fields[0].FieldType;

            IList<Composition> list = new List<Composition>();
            var typeList = typeof(IList<Composition>);

            Assert.That(typeOfField, Is.EqualTo(typeList), () => "The Compostion Class should have one private field of type List<Composition>");
        }

        [MonitoredTest("Should have constructor who fills the private field")]
        public void _02_ShouldHaveConstructorWhoFillsThePrivateField()
        {
            CompositionSearcher searcher = new CompositionSearcher();
            Assert.That(typeof(CompositionSearcher).HasDefaultConstructor(), Is.True, () => "CompositionSearcher Class should have a parameterless default Constructor");
            FieldInfo field = typeof(CompositionSearcher).GetField("_allCompositions", BindingFlags.NonPublic | BindingFlags.Instance);
            List<Composition> compositions = (List<Composition>)field.GetValue(searcher);
            Assert.That(compositions.Count, Is.EqualTo(4), () => "Constructor should call GetCompositions to fill the private member field");

        }

        [MonitoredTest("Should have a SearchMusic method")]
        public void _03_ShouldHaveASearchMusicMethod()
        {
            MethodInfo method = typeof(CompositionSearcher).GetMethod("SearchMusic");
            Assert.That(method, Is.Not.Null, () => "CompositionSearcher class should have a 'SearchMusic' method'");
            
            var type1 = typeof(IList<Composition>).ToString().ToLower();
            var returnType = method.ReturnType.ToString().ToLower();
            
            Assert.That(type1, Is.EqualTo(returnType), () => "The return type of the 'SearchMusic' method is not correct");

            ParameterInfo[] parameters = method.GetParameters();
            Assert.That(parameters[0].ParameterType.ToString().ToLower(), Is.EqualTo(typeof(CompositionFilterDelegate).ToString().ToLower()), () => "The type of the first parameter of the 'SearchMusic' method has to be CompositionFilterDelegate");
            Assert.That(parameters[1].ParameterType.ToString().ToLower(), Is.EqualTo(typeof(String).ToString().ToLower()), () => "The type of the second parameter of the 'SearchMusic' method has to be String");
        }      
    }
}


