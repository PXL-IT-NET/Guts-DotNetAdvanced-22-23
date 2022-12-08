using Guts.Client.Core;
using InternshipsAdmin.Domain;
using InternshipsAdmin.Infrastructure;

namespace InternshipsAdmin.Tests
{
    [ExerciseTestFixture("dotnet2", "H08", "Exercise01", @"InternshipsAdmin.Infrastructure\StudentsRepository.cs")]
    internal class StudentsRepositoryTests : DatabaseTests
    {
        [MonitoredTest("StudentsRepository - GetAll - Should return all the students from the database")]
        public void GetAll_ShouldReturnAllStudentsFromDb()
        {
            var originalAmountOfStudents = 0;
            Student someStudent, someSecondStudent;

            using (var context = CreateDbContext(true))
            {
                originalAmountOfStudents = context.Set<Student>().Count();

                //Arrange
                someStudent = new Student(Guid.NewGuid().ToString())
                {
                    Department = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString(),
                    Phone = Guid.NewGuid().ToString()
                };
                someSecondStudent = new Student(Guid.NewGuid().ToString())
                {
                    Department = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString(),
                    Phone = Guid.NewGuid().ToString()
                };

                context.Add(someStudent);
                context.Add(someSecondStudent);
                context.SaveChanges();
            }

            using (var context = CreateDbContext(false))
            {
                var repo = new StudentsRepository(context);

                //Act
                var allStudents = repo.GetAll();

                //Assert
                Assert.That(allStudents, Has.Count.EqualTo(originalAmountOfStudents + 2), () => "Not all students in the database are returned.");
                var expectedCompany = allStudents.FirstOrDefault(game => game.Name == someStudent.Name);
                Assert.That(expectedCompany, Is.Not.Null, () => "Not all students in the database are returned.");
            }
        }

        [MonitoredTest("StudentsRepository - GetStudentsWithouSupervisor - Should return all the students who do not have a supervisor from the database")]
        public void GetStudentsWithoutSupervisor_ShouldReturnAllStudentsWithoutSupervisorFromDb()
        {
            var originalAmountOfStudents = 0;
            Student someStudent, someSecondStudent;

            using (var context = CreateDbContext(true))
            {
                originalAmountOfStudents = context.Set<Student>().Count();

                //Arrange
                someStudent = new Student(Guid.NewGuid().ToString())
                {                    
                    Department = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString(),
                    Phone = Guid.NewGuid().ToString()
                };
                someSecondStudent = new Student(Guid.NewGuid().ToString())
                {                    
                    Department = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString(),
                    Phone = Guid.NewGuid().ToString()
                };

                context.Add(someStudent);
                context.Add(someSecondStudent);
                context.SaveChanges();
            }

            using (var context = CreateDbContext(false))
            {
                var repo = new StudentsRepository(context);

                //Act
                var allStudents = repo.GetStudentsWithoutSupervisor();

                //Assert
                Assert.That(allStudents, Has.Count.EqualTo(originalAmountOfStudents+2), () => "Not all student in the database are returned.");

                foreach(var student in allStudents)
                    Assert.That(student.Supervisor, Is.Null, () => "Not all students returned have an empty supervisor.");
            }
        }
    }
}
