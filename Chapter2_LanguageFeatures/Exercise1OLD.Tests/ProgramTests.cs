using Chapter2_LanguageFeatures;
using Guts.Client.Core.TestTools;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Data;
using System.Reflection;

namespace Exercise1.Tests
{
    public class ProgramTests
    {
        private TypeInfo _searchMusicDelegateTypeInfo;        
        private List<LocalVariableInfo> _delegates;
        private string _programClassContent;
        private StringWriter _writer;
        private StringReader _reader;
        private MethodBody _body;
        Assembly _assembly;

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            var _type = typeof(List<Music>);
            _assembly = Assembly.GetAssembly(typeof(Program));
            _searchMusicDelegateTypeInfo = _assembly.DefinedTypes.FirstOrDefault(t =>
            {
                if (!typeof(MulticastDelegate).IsAssignableFrom(t)) return false;
                
                //check signature (must return List<Music> and have a List<Music> and a string parameter)
                var methodInfo = t.DeclaredMethods.First(p => p.Name == "Invoke");
                //_delegateMethods.Add(methodInfo);
                if (methodInfo.ReturnType.Name.ToLower() != _type.Name.ToLower()) return false;
                ParameterInfo[] parameters = methodInfo.GetParameters();
                if (parameters.Length != 2) return false;
                if (parameters[0].ParameterType != typeof(List<Music>)) return false;
                if (parameters[1].ParameterType != typeof(string)) return false;
                return true;
            });
            
        }

        [SetUp]
        public void BeforeEachTest()
        {          
            _writer = new StringWriter();
            _reader = new StringReader("is\nis\n1800\n\t");
            
            Console.SetOut(_writer);
            Console.SetIn(_reader);
        }

        [Test]
        public void _01_ThereShouldBeAMusicSearchDelegateTypeDefined()
        {
            AssertMusicSearchDelegateIsDefinedCorrectly();
        }

        [Test]
        public void _02_ShouldDeclare3MusicSearchDelegatesVarsInMainMethod()
        {
            Assert.That(_delegates.Count, Is.EqualTo(3), "You have to declare 3 delegate variables.");                  
        }

        [Test]
        public void _03_ShouldCallThe3DelegatesInMain()
        {
            // hier zou ik willen testen naar welke Methods de delegates verwijzen en of deze worden aangeroepen
            // heb jij een idee, want heb al aantal uur zitten zoeken...
         

        }

        [Test]
        public void _04_ShouldWriteOutputToConsole()
        {        
            //execute the program
            Program.Main(null);

            string consoleOutput = _writer.ToString();
            Assert.That(consoleOutput, Is.Not.Empty, "Nothing has been written to the console.");

            string[] consoleLines = consoleOutput.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            Assert.That(consoleLines.Count(), Is.GreaterThanOrEqualTo(29),
                "At least 29 lines, written to the console, should contain the text 'music'.");

        }
        [Test]
        public void _05_Program_ShouldUseToCenturyExtensionMethod()
        {
            //AssertProgramIsInstantiated();
            Program.Main(null); //check if the program runs without exceptions

            Assert.That(CallsMemberMethod("ToCentury"), Is.True, "Cannot find an invocation of the 'NextBalloon' method of a 'Random' instance.");
        }

        private void AssertMusicSearchDelegateIsDefinedCorrectly()
        {
            Assert.IsNotNull(_searchMusicDelegateTypeInfo,
                "Cannot find a delegate type definition that supports methods that return a List of Music objects and accept a List of Music objects an a string parameter. " +
                "Define the type in 'WriteDelegate.cs'.");
        }

        
        private bool CallsMemberMethod(string methodName)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(_programClassContent);
            var root = syntaxTree.GetRoot();
            return root
                .DescendantNodes()
                .OfType<MemberAccessExpressionSyntax>()
                .Any(memberAccess => memberAccess.Name.ToString().ToLower() == methodName.ToLower());

        }
    }
}