using System.Reflection;
using Guts.Client.Core;

namespace Exercise2.Tests;

[ExerciseTestFixture("dotNet2", "H02", "Exercise02", @"Exercise2\NewStudentEventHandler.cs;Exercise2\StudentEventArgs.cs")]
public class NewStudentEventHandlerTests
{

    [MonitoredTest("NewStudentEventHandler - Should be correctly defined")]
    public void _01_NewStudentEventHandlerShouldBeCorrectlyDefined()
    {
        TypeInfo? newStudentEventHandlerTypeInfo = TestHelper.GetNewStudentEventHandlerTypeInfo();
        Assert.That(newStudentEventHandlerTypeInfo, Is.Not.Null,
            "Cannot find a delegate type definition that supports methods that return void and accept an object and a StudentEventArgs parameter.");
    }
}