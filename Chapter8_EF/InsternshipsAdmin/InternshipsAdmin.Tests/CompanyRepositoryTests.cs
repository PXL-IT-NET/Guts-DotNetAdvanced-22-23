using Guts.Client.Core;
using InternshipsAdmin.Domain;
using InternshipsAdmin.Infrastructure;


namespace InternshipsAdmin.Tests
{
    [ExerciseTestFixture("dotnet2", "H08", "Exercise01",
    @"InternshipsAdmin.Infrastructure\CompanyRepository.cs")]
    internal class CompanyRepositoryTests : DatabaseTests
    {
        private static Random _random = new Random();

        [MonitoredTest("CompanyRepository - Add - Should add a new company to the database")]
        public void Add_ShouldAddNewCompanyToDb()
        {
            IList<Company> allOriginalCompanies;
            using (var context = CreateDbContext())
            {
                allOriginalCompanies = context.Set<Company>().ToList();
            }

            Company newCompany = new Company()
            {
                Name = Guid.NewGuid().ToString(),
                Address = Guid.NewGuid().ToString(),
                City = Guid.NewGuid().ToString(),
                Zip = Guid.NewGuid().ToString()
            };

            using (var context = CreateDbContext())
            {
                var repo = new CompanyRepository(context);

                //Act
                repo.Add(newCompany);

                //Assert
                var allCompanies = context.Set<Company>().ToList();
                Assert.That(allCompanies, Has.Count.EqualTo(allOriginalCompanies.Count + 1),
                    "No company is added in the database.");
                var addedCompany = allCompanies.FirstOrDefault(c => c.Name == newCompany.Name);
                Assert.That(addedCompany, Is.Not.Null,
                    "No customer with the added name can be found in the database afterwards.");
                Assert.That(addedCompany.CompanyId, Is.GreaterThan(0),
                    "The CompanyId of the added customer must be greater than zero.");
                Assert.That(addedCompany.Address, Is.EqualTo(newCompany.Address),
                    "The 'Address' is not saved correctly.");
                Assert.That(addedCompany.City, Is.EqualTo(newCompany.City),
                    "The 'City' is not saved correctly.");
                Assert.That(addedCompany.Name, Is.EqualTo(newCompany.Name),
                    "The 'Name' is not saved correctly.");
                Assert.That(addedCompany.Zip, Is.EqualTo(newCompany.Zip),
                    "The 'Zip' is not saved correctly.");
            }
        }

        [MonitoredTest("CompanyRepository - GetAll - Should return all the companies from the database")]
        public void GetAll_ShouldReturnAllCompaniesFromDb()
        {
            var originalAmountOfCompanies = 0;
            Company someCompany, someSecondCompany;

            using (var context = CreateDbContext(true))
            {
                originalAmountOfCompanies = context.Set<Company>().Count();

                //Arrange
                someCompany = new Company()
                {
                    Name = Guid.NewGuid().ToString(),
                    Address = Guid.NewGuid().ToString(),
                    City = Guid.NewGuid().ToString(),
                    Zip = Guid.NewGuid().ToString()
                };
                someSecondCompany = new Company()
                {
                    Name = Guid.NewGuid().ToString(),
                    Address = Guid.NewGuid().ToString(),
                    City = Guid.NewGuid().ToString(),
                    Zip = Guid.NewGuid().ToString()
                };

                context.Add(someCompany);
                context.Add(someSecondCompany);
                context.SaveChanges();
            }

            using (var context = CreateDbContext(false))
            {
                var repo = new CompanyRepository(context);

                //Act
                var allCompanies = repo.GetAll();

                //Assert
                Assert.That(allCompanies, Has.Count.EqualTo(originalAmountOfCompanies + 2), () => "Not all companies in the database are returned.");
                var expectedCompany = allCompanies.FirstOrDefault(c => c.Name == someCompany.Name);
                Assert.That(expectedCompany, Is.Not.Null, () => "Not all companies in the database are returned.");
            }
        }

        [MonitoredTest("CompanyRepository - GetContactOfCompany - Should return the Contact of a given Company")]
        public void GetContactOfCompany_ShouldReturnTheCompanyContactFromDb()
        {
            Company someCompany;
            //Arrange

            using (var context = CreateDbContext(false))
            {
                someCompany = new Company()
                {
                    Name = Guid.NewGuid().ToString(),
                    Address = Guid.NewGuid().ToString(),
                    City = Guid.NewGuid().ToString(),
                    Zip = Guid.NewGuid().ToString()

                };

                var contact = new Contact(Guid.NewGuid().ToString())
                {
                    Phone = Guid.NewGuid ().ToString(),
                    Email = Guid.NewGuid().ToString(),
                    Prefix = Guid.NewGuid().ToString(),
                    Company = someCompany,
                    CompanyId = someCompany.CompanyId
                };

                context.Add(contact);
                context.SaveChanges();
            }

            using (var context = CreateDbContext(false))
            {
                var repo = new CompanyRepository(context);

                //Act
                var contact = repo.GetContactOfCompany(someCompany.CompanyId);

                //Assert
                Assert.That(contact, Is.Not.Null, () => "Contact of the company in the database not returned.");
                Assert.That(contact.Id, Is.EqualTo(contact.Id));

            }
        }

        [MonitoredTest("CompanyRepository - GetSupervisorsOfCompany - Should return all the supervisors of a given company from the database")]
        public void GetSupervisorsOfCompany_ShouldReturnTheCompanySupervisorsFromDb()
        {
            int originalAmountOfCompanies = 0;
            Company someCompany;
            int companyId;

            //Arrange

            using (var context = CreateDbContext(false))
            {
                originalAmountOfCompanies = context.Set<Company>().Count();

                companyId = originalAmountOfCompanies + 1;

                someCompany = new Company()
                {                    
                    Name= Guid.NewGuid().ToString(),
                    Address = Guid.NewGuid().ToString(),
                    City = Guid.NewGuid().ToString(),
                    Zip = Guid.NewGuid().ToString()

                };

                List<Supervisor> supervisors = new List<Supervisor>();

                var supervisor = new Supervisor(Guid.NewGuid().ToString())
                {                
                    Phone = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString(),
                    JobTitle = Guid.NewGuid().ToString(),
                    Specialism = Guid.NewGuid().ToString()
                };
                someCompany.Supervisors.Add(supervisor);


                var supervisor2 = new Supervisor(Guid.NewGuid().ToString())
                {
                    Phone = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString(),
                    JobTitle = Guid.NewGuid().ToString(),
                    Specialism = Guid.NewGuid().ToString()
                };
                supervisors.Add(supervisor2);

                someCompany.Supervisors.Add(supervisor2);

                context.Add(someCompany);
                context.SaveChanges();
            }

            using (var context = CreateDbContext(false))
            {
                var repo = new CompanyRepository(context);

                //Act
                var supervisors = repo.GetSupervisorsOfCompany(someCompany.CompanyId);

                //Assert
                Assert.That(supervisors, Is.Not.Null, () => "Supervisors of the company in the database not returned.");
                Assert.That(supervisors.Count(), Is.EqualTo(2));
            }
        }

        [MonitoredTest("CompanyRepository - GetStudentsOfCompany - Should return all the students of the supervisors of a given company from the database")]
        public void GetStudentsOfCompany_ShouldReturnTheCompanyStudentsFromDb()
        {
            Company someCompany;

            //Arrange

            using (var context = CreateDbContext(false))
            {
                someCompany = new Company()
                {
                    Name = Guid.NewGuid().ToString(),
                    Address = Guid.NewGuid().ToString(),
                    City = Guid.NewGuid().ToString(),
                    Zip = Guid.NewGuid().ToString()

                };

                var supervisor = new Supervisor(Guid.NewGuid().ToString())
                {
                    Phone = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString(),
                    JobTitle = Guid.NewGuid().ToString(),
                    Specialism = Guid.NewGuid().ToString()
                };
                someCompany.Supervisors.Add(supervisor);


                var student = new Student(Guid.NewGuid().ToString())
                {
                    Department = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString(),
                    Phone = Guid.NewGuid().ToString(),
                    Supervisor = supervisor,
                    SupervisorId = supervisor.Id
                };

                supervisor.Students.Add(student);
                var student2 = new Student(Guid.NewGuid().ToString())
                {
                    Department = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString(), 
                    Phone = Guid.NewGuid().ToString(),
                    Supervisor = supervisor,
                    SupervisorId = supervisor.Id
                };
                supervisor.Students.Add(student2);

                context.Add(someCompany);

                context.SaveChanges();
            }

            using (var context = CreateDbContext(false))
            {
                var repo = new CompanyRepository(context);

                //Act
                var students = repo.GetStudentsOfCompany(someCompany.CompanyId);

                //Assert
                Assert.That(students, Is.Not.Null, () => "Supervisors of the company in the database not returned.");
                Assert.That(students.Count(), Is.EqualTo(2));

            }
        }

        [MonitoredTest("CompanyRepository - AddStudentWithSupervisor - Should add an existing supervisor to an existing student to the database")]
        public void AddStudentWithSupervisor_ShouldAddAStudentWithSupervisorToTheDatabase()
        {
            Student stud;
            Supervisor supervisor;
            using (var context = CreateDbContext(false))
            {
                var company = new Company();
                context.Add(company);
                context.SaveChanges();

                supervisor = new Supervisor(Guid.NewGuid().ToString())
                {
                    Email = Guid.NewGuid().ToString(),
                    Phone = Guid.NewGuid().ToString(),
                    JobTitle = Guid.NewGuid().ToString(),
                    Specialism = Guid.NewGuid().ToString(),
                    Company= company
                };

                context.Add(supervisor);
                context.SaveChanges();

                //Arrange                       
                stud = new Student(Guid.NewGuid().ToString())
                {
                    Phone = Guid.NewGuid().ToString(),
                    Department = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString()
                };

                
                context.Add(stud);
                context.SaveChanges();

                var repo = new CompanyRepository(context);

                //Act
                repo.AddStudentWithSupervisorForCompany(stud, supervisor);
            }
            using (var context = CreateDbContext(false))
            {
                var student = context.Set<Student>().Where(s => s.Id == stud.Id).First();

                //Assert
                Assert.That(student, Is.Not.Null, () => "Student should be added to the database");
                Assert.That(student.SupervisorId, Is.EqualTo(supervisor.Id));

            }
        }

        [MonitoredTest("CompanyRepository - RemoveStudentFromSupervisor - Should remove the supervisor from an existing student in the database")]
        public void RemoveStudentFromSupervisor_ShouldRemoveAStudentWithSupervisorToTheDatabase()
        {
            Student stud;
            Supervisor supervisor;
            

            using (var context = CreateDbContext(false))
            {
                var company = new Company(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
                context.Add(company);
                context.SaveChanges();

                supervisor = new Supervisor(Guid.NewGuid().ToString())
                {
                    Email = Guid.NewGuid().ToString(),
                    Phone = Guid.NewGuid().ToString(),
                    JobTitle = Guid.NewGuid().ToString(),
                    Specialism = Guid.NewGuid().ToString(),
                    Company = company
                };

                context.Add(supervisor);
                context.SaveChanges();

                //Arrange                       
                stud = new Student(Guid.NewGuid().ToString())
                {
                    Phone = Guid.NewGuid().ToString(),
                    Department = Guid.NewGuid().ToString(),
                    Email = Guid.NewGuid().ToString(),
                    SupervisorId = supervisor.Id,
                    Supervisor = supervisor
                };

                context.Add(stud);
                context.SaveChanges();

                var repo = new CompanyRepository(context);

                //Act
                repo.RemoveStudentFromSupervisor(stud, supervisor);
            }
            using (var context = CreateDbContext(false))
            {
                var student = context.Set<Student>().Where(s => s.Id == stud.Id).First();

                //Assert
                Assert.That(student, Is.Not.Null, () => "Student should be added to the database");
                Assert.That(student.SupervisorId, Is.Null);

            }
        }


    }
}

