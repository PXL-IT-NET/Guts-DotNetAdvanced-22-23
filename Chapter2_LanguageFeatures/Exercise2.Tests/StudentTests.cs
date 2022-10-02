using Guts.Client.Core;

namespace Exercise2.Tests;

[ExerciseTestFixture("dotNet2", "H02", "Exercise02", @"Exercise2\Student.cs")]
public class StudentTests
{

    [MonitoredTest("Student - Should have read-only properties")]
    public void _01_ShouldHaveReadOnlyProperties()
    {
        var firstNameProperty = typeof(Student).GetProperty(nameof(Student.FirstName));
        Assert.That(firstNameProperty.CanWrite, Is.False, "FirstName is not read-only");
        Assert.That(TestHelper.IsMarkedAsNullable(firstNameProperty), Is.False, "FirstName should not be nullable");

        var lastNameProperty = typeof(Student).GetProperty(nameof(Student.LastName));
        Assert.That(lastNameProperty.CanWrite, Is.False, "LastName is not read-only");
        Assert.That(TestHelper.IsMarkedAsNullable(lastNameProperty), Is.False, "LastName should not be nullable");

        var departmentProperty = typeof(Student).GetProperty(nameof(Student.Department));
        Assert.That(departmentProperty.CanWrite, Is.False, "Department is not read-only");
        Assert.That(TestHelper.IsMarkedAsNullable(departmentProperty), Is.True, "Department should be nullable");
    }

    [MonitoredTest("Student - Constructor - Should initialize properties")]
    public void _02_Constructor_ShouldInitializeProperties()
    {
        string firstName = Guid.NewGuid().ToString();
        string lastName = Guid.NewGuid().ToString();
        string department = Guid.NewGuid().ToString();

        var student = new Student(firstName, lastName, department);

        Assert.That(student.FirstName, Is.EqualTo(firstName), "FirstName is not initialized correctly");
        Assert.That(student.LastName, Is.EqualTo(lastName), "LastName is not initialized correctly");
        Assert.That(student.Department, Is.EqualTo(department), "Department is not initialized correctly");
    }

    [MonitoredTest("ToString - Should return student info")]
    [TestCase("Jane", "Doe", "Digital", "Jane Doe - Digital")]
    [TestCase("John", "Doe", null, "John Doe - /")]
    public void _03_ToString_ShouldReturnStudentInfo(string firstName, string lastName, string? department, string expected)
    {
        string result = new Student(firstName, lastName, department).ToString();
        Assert.That(result, Is.EqualTo(expected));
    }
}