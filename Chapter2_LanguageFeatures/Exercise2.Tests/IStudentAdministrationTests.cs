using System.Reflection;
using Guts.Client.Core;

namespace Exercise2.Tests;

[ExerciseTestFixture("dotNet2", "H02", "Exercise02", @"Exercise2\IStudentAdministration.cs")]
public class IStudentAdministrationTests
{
    private TypeInfo? _newStudentEventHandlerTypeInfo;
    private EventInfo? _eventInfo;


    [OneTimeSetUp]
    public void BeforeAllTests()
    {
        Type type = typeof(IStudentAdministration);
        _newStudentEventHandlerTypeInfo = TestHelper.GetNewStudentEventHandlerTypeInfo();

        if (_newStudentEventHandlerTypeInfo != null)
        {
            _eventInfo = type.GetEvents().FirstOrDefault(t => t.EventHandlerType == _newStudentEventHandlerTypeInfo.AsType());

        }
    }

    [MonitoredTest("IStudentAdministration - Should have a public event defined to announce new student registrations")]
    public void _01_ShouldHaveAPublicEventToAnnounceNewStudentRegistrations()
    {
        Assert.That(_newStudentEventHandlerTypeInfo, Is.Not.Null, "Make sure the NewStudentEventHandler tests are green first");
        Assert.That(_eventInfo, Is.Not.Null);
    }
}