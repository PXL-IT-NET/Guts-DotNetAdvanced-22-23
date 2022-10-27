using System.Reflection;
using LinqExamples.Tests;

namespace Guts.Tests
{
    [ExerciseTestFixture("dotnet2", "H07", "Exercise01",
        @"LinqExamples\SelectExamples.cs;
LinqExamples\WhereExamples.cs;
LinqExamples\OrderByExamples.cs;
LinqExamples\GroupExamples.cs;
LinqExamples\JoinExamples.cs;")]
    public class LinqExampleTestsTests
    {
        private Dictionary<string, string> _contents = null!;

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            _contents = new Dictionary<string, string>
            {
                {
                    "SelectExamples.cs",
                    CodeCleaner.StripComments(Solution.Current.GetFileContent(@"LinqExamples\SelectExamples.cs").Trim())
                        .Replace(" ", "").ToLower()
                },
                {
                    "WhereExamples.cs",
                    CodeCleaner.StripComments(Solution.Current.GetFileContent(@"LinqExamples\WhereExamples.cs").Trim())
                        .Replace(" ", "").ToLower()
                },
                {
                    "OrderByExamples.cs",
                    CodeCleaner.StripComments(Solution.Current.GetFileContent(@"LinqExamples\OrderByExamples.cs").Trim())
                        .Replace(" ", "").ToLower()
                },
                {
                    "GroupExamples.cs",
                    CodeCleaner.StripComments(Solution.Current.GetFileContent(@"LinqExamples\GroupExamples.cs").Trim())
                        .Replace(" ", "").ToLower()
                },
                {
                    "JoinExamples.cs",
                    CodeCleaner.StripComments(Solution.Current.GetFileContent(@"LinqExamples\JoinExamples.cs").Trim())
                        .Replace(" ", "").ToLower()
                }
            };
        }

        [MonitoredTest("Should not have changed test files"), Order(1)]
        public void _01_ShouldNotHaveChangedTestFiles()
        {
            var hash = Solution.Current.GetFileHash(@"LinqExamples.Tests\1_SelectExamplesTests.cs");
            Assert.That(hash, Is.EqualTo("05-A8-20-F7-06-DC-A5-6B-F6-3D-38-2A-0C-53-65-1B"), "'1_SelectExamplesTests.cs' has been changed.");

            hash = Solution.Current.GetFileHash(@"LinqExamples.Tests\2_WhereExamplesTests.cs");
            Assert.That(hash, Is.EqualTo("F1-08-7B-D5-05-BA-C5-EB-A6-1F-4D-96-D3-BB-91-2A"), "'2_WhereExamplesTests.cs' has been changed.");

            hash = Solution.Current.GetFileHash(@"LinqExamples.Tests\3_OrderByExamplesTests.cs");
            Assert.That(hash, Is.EqualTo("66-1B-C2-FD-AA-D6-F9-CE-D1-7E-E5-E5-A0-7D-49-6B"), "'3_OrderByExamplesTests.cs' has been changed.");

            hash = Solution.Current.GetFileHash(@"LinqExamples.Tests\4_GroupExamplesTests.cs");
            Assert.That(hash, Is.EqualTo("64-19-01-49-95-04-8B-B8-16-FF-8E-8C-A9-FE-12-6D"), "'4_GroupExamplesTests.cs' has been changed.");

            hash = Solution.Current.GetFileHash(@"LinqExamples.Tests\5_JoinExamplesTests.cs");
            Assert.That(hash, Is.EqualTo("FB-B4-18-38-F4-51-E0-0F-0A-BB-FA-A8-5C-37-F7-28"), "'5_JoinExamplesTests.cs' has been changed.");
        }

        [MonitoredTest("Should use LINQ"), Order(2)]
        public void _02_ShouldUseLinq()
        {
            AssertUsesLinq("SelectExamples.cs");
            AssertUsesLinq("WhereExamples.cs");
            AssertUsesLinq("OrderByExamples.cs");
            AssertUsesLinq("GroupExamples.cs");
            AssertUsesLinq("JoinExamples.cs");
        }

        [MonitoredTest("All select example tests should pass"), Order(3)]
        public void _03_SelectExampleTests_ShouldAllPass()
        {
            AssertUsesLinq("SelectExamples.cs");
            var testClassInstance = new SelectExamplesTests();
            AssertAllTestMethodsPass(testClassInstance);
        }

        [MonitoredTest("All where example tests should pass"), Order(4)]
        public void _04_WhereExampleTests_ShouldAllPass()
        {
            AssertUsesLinq("WhereExamples.cs");
            var testClassInstance = new WhereExamplesTests();
            AssertAllTestMethodsPass(testClassInstance);
        }

        [MonitoredTest("All orderby example tests should pass"), Order(5)]
        public void _05_OrderByExampleTests_ShouldAllPass()
        {
            AssertUsesLinq("OrderByExamples.cs");
            var testClassInstance = new OrderByExamplesTests();
            AssertAllTestMethodsPass(testClassInstance);
        }

        [MonitoredTest("All group example tests should pass"), Order(6)]
        public void _06_GroupExampleTests_ShouldAllPass()
        {
            AssertUsesLinq("GroupExamples.cs");
            var testClassInstance = new GroupExamplesTests();
            AssertAllTestMethodsPass(testClassInstance);
        }

        [MonitoredTest("All join example tests should pass"), Order(7)]
        public void _07_JoinExampleTests_ShouldAllPass()
        {
            AssertUsesLinq("JoinExamples.cs");
            var testClassInstance = new JoinExamplesTests();
            AssertAllTestMethodsPass(testClassInstance);
        }

        private void AssertUsesLinq(string sourceFileName)
        {
            string content = _contents[sourceFileName];

            Assert.That(content, Does.Not.Contain("for("), $"A for-loop is used in '{sourceFileName}'. " +
                                                           "This is not necessary when LINQ is used.");

            Assert.That(content, Does.Not.Contain("foreach("), $"A foreach-loop is used in '{sourceFileName}'. " +
                                                               "This is not necessary when LINQ is used.");

            Assert.That(content, Does.Not.Contain("while("), $"A while-loop is used in '{sourceFileName}'. " +
                                                             "This is not necessary when LINQ is used.");
        }

        private void AssertAllTestMethodsPass(object testClassInstance)
        {
            var testClassType = testClassInstance.GetType();

            var setupMethod = testClassType.GetMethods()
                .FirstOrDefault(m => m.GetCustomAttribute<SetUpAttribute>() != null);

            var testMethodInfos = testClassType.GetMethods().Where(m => m.GetCustomAttribute<TestAttribute>() != null)
                .ToList();

            foreach (var testMethodInfo in testMethodInfos)
            {
                if (setupMethod != null)
                {
                    setupMethod.Invoke(testClassInstance, Array.Empty<object>());
                }
                AssertTestMethodPasses(testClassInstance, testMethodInfo);
            }
        }

        private void AssertTestMethodPasses(object testClassInstance, MethodInfo testMethod)
        {
            Assert.That(() => testMethod.Invoke(testClassInstance, Array.Empty<object>()), Throws.Nothing,
                () => $"{testMethod.Name}() should pass, but doesn't.");
        }
    }
}