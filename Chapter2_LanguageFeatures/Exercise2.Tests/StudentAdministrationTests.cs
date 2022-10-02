using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using System.Reflection;

namespace Exercise2.Tests;

[ExerciseTestFixture("dotNet2", "H02", "Exercise02", @"Exercise2\StudentAdministration.cs")]

public class StudentAdministrationTests
{
    private TypeInfo? _newStudentEventHandlerTypeInfo;
    private EventInfo? _eventInfo;
    private Student _newStudent;
    private bool _eventWasRaised;
    private string _studentAdministrationClassContent;


    [OneTimeSetUp]
    public void BeforeAllTests()
    {
        _studentAdministrationClassContent = Solution.Current.GetFileContent(@"Exercise2\StudentAdministration.cs")!;

        Type type = typeof(StudentAdministration);
        _newStudentEventHandlerTypeInfo = TestHelper.GetNewStudentEventHandlerTypeInfo();

        if (_newStudentEventHandlerTypeInfo != null)
        {
            _eventInfo = type.GetEvents().FirstOrDefault(t => t.EventHandlerType == _newStudentEventHandlerTypeInfo.AsType());
        }
    }

    [SetUp]
    public void BeforeEachTest()
    {
        _newStudent = new Student("John", "Doe", "Digital");
        _eventWasRaised = false;
    }

    [MonitoredTest("StudentAdministration - Should implement IStudentAdministration")]
    public void _01_ShouldImplementIStudentAdministration()
    {
        Assert.That(typeof(StudentAdministration).IsAssignableTo(typeof(IStudentAdministration)));
        Assert.That(_eventInfo, Is.Not.Null, "Make sure the tests on IStudentAdministration are green first");
    }

    [MonitoredTest("StudentAdministration - Should not have a dependency on the BlackBoard class"), Order(1)]
    public void _02_ShouldNotHaveADependencyOnTheBlackBoardClass()
    {
        Assert.That(_studentAdministrationClassContent.ToLower().Contains("blackboard"), Is.False);
    }

    [MonitoredTest("StudentAdministration - RegisterStudent - Should add the student to a (private) list of students")]
    public void _03_RegisterStudent_ShouldAddTheStudentToAPrivateListOfStudents()
    {
        _01_ShouldImplementIStudentAdministration();
        var administration = new StudentAdministration();
        var allStudentsBefore = GetAllStudents(administration);
        Assert.That(allStudentsBefore.Count, Is.EqualTo(0), "Before the registration the number of students should be zero");

        administration.RegisterStudent(_newStudent);

        var allStudentsAfter = GetAllStudents(administration);
        Assert.That(allStudentsAfter.Count, Is.EqualTo(1), "After the registration the number of students should be one");
        Assert.That(allStudentsAfter[0], Is.SameAs(_newStudent),
            "The added student should be the same instance that was passed into the RegisterStudent method");
    }

    [MonitoredTest("StudentAdministration - RegisterStudent - Should announce the new student registration")]
    public void _04_RegisterStudent_ShouldAnnounceTheNewStudentRegistration()
    {
        _01_ShouldImplementIStudentAdministration();

        var administration = new StudentAdministration();

        MethodInfo handlerMethodInfo =
            typeof(StudentAdministrationTests).GetMethod("TestHandler",
                BindingFlags.NonPublic | BindingFlags.Instance);
        Delegate d = Delegate.CreateDelegate(_newStudentEventHandlerTypeInfo.AsType(), this, handlerMethodInfo);

        _eventInfo.AddEventHandler(administration, d);

        administration.RegisterStudent(_newStudent);
        Assert.That(_eventWasRaised, Is.True, "The event was not raised");
    }

    private void TestHandler(object sender, StudentEventArgs args)
    {
        _eventWasRaised = true;
        Assert.That(args, Is.Not.Null, "The event argument should not be null");
        Assert.That(args.Student, Is.SameAs(_newStudent),
            "The student in the event argument should be the same student instance that was registered");
    }

    private IList<Student> GetAllStudents(StudentAdministration target)
    {
        Type type = typeof(StudentAdministration);
        var allStudentsFieldInfo = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .FirstOrDefault(f => f.FieldType == typeof(IList<Student>));

        Assert.That(allStudentsFieldInfo, Is.Not.Null, "Cannot find a private field of type IList<Student>");
        Assert.That(allStudentsFieldInfo.IsInitOnly, "The private field that holds all students should be read-only");

        return allStudentsFieldInfo.GetValue(target) as IList<Student> ?? new List<Student>();
    }
}