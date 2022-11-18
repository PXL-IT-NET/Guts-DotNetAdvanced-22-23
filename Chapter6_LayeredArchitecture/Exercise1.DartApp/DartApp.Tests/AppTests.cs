using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;
using DartApp.Infrastructure.Storage;
using DartApp.AppLogic;
using DartApp.Presentation;

namespace DartApp.Tests
{
    [ExerciseTestFixture("dotnet2", "H06", "Exercise01",
    @"DartApp.Presentation\App.xaml.cs")]
    public class AppTests
    {
        private string _appClassContent = null!;

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            _appClassContent = Solution.Current.GetFileContent(@"DartApp.Presentation\App.xaml.cs");
        }

        [MonitoredTest("App - OnStartup should inject a player service instance into the MainWindow")]
        public void _01_OnStartup_ShouldInjectAPlayerServiceInstanceIntoTheMainWindowAndShowIt()
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(_appClassContent);
            var root = syntaxTree.GetRoot();
            MethodDeclarationSyntax? onStartupMethod = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(md => md.Identifier.ValueText.Equals("OnStartup"));

            Assert.That(onStartupMethod, Is.Not.Null, "Cannot find a method 'OnStartup' in App.xaml.cs");

            List<ObjectCreationExpressionSyntax> objectCreations = onStartupMethod!.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().ToList();

            ObjectCreationExpressionSyntax? playerFileRepositoryCreation = objectCreations.FirstOrDefault(oc => oc.Type.ToString() == nameof(PlayerFileRepository));
            Assert.That(playerFileRepositoryCreation, Is.Not.Null, "Cannot find a statement in 'OnStartup' where a new instance of PlayerFileRepository is created.");

            ObjectCreationExpressionSyntax? playerServiceCreation = objectCreations.FirstOrDefault(oc => oc.Type.ToString() == nameof(PlayerService));
            Assert.That(playerFileRepositoryCreation, Is.Not.Null, "Cannot find a statement in 'OnStartup' where a new instance of PlayerService is created.");

            ObjectCreationExpressionSyntax? mainWindowCreation = objectCreations.FirstOrDefault(oc => oc.Type.ToString() == nameof(MainWindow));
            Assert.That(mainWindowCreation, Is.Not.Null, "Cannot find a statement in 'OnStartup' where a new instance of MainWindow is created.");

            var bodyBuilder = new StringBuilder(); //no pun intended :)
            foreach (var statement in onStartupMethod.Body!.Statements)
            {
                bodyBuilder.AppendLine(statement.ToString());
            }
            string body = bodyBuilder.ToString();

            Assert.That(body, Contains.Substring("Environment.GetFolderPath"),
                "The folder to save players in, should be in the special 'AppData' directory. Use the 'Environment' class to retrieve the path of that directory.");

            Assert.That(body, Contains.Substring("Environment.SpecialFolder.ApplicationData"),
                "The folder to save players in, should be in the special 'AppData' directory. Use the 'Environment.SpecialFolder' enum.");

            Assert.That(body, Contains.Substring("Path.Combine(").And.Contains(@"""DartApp"")"),
                "The folder to save players in, should be a subdirectory 'DartApp' in the special 'AppData' directory. " +
                "Use the static 'Combine' method of the 'System.IO.Path' class to create a string that holds the complete directory path.");

            Assert.That(body, Contains.Substring(".Show();"),
                "The MainWindow is instantiated, but not shown in the 'OnStartup' method.");
        }
    }
}
