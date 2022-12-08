using InternshipsAdmin.AppLogic.Contracts;
using InternshipsAdmin.Domain;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace InternshipsAdmin.Infrastructure
{
    internal class CompanyRepository : ICompanyRepository
    {
        private readonly InternshipsContext _context;

        public CompanyRepository(InternshipsContext context)
        {
            _context = context;
        }

        public void Add(Company company)
        {
            _context.Add(company);
            _context.SaveChanges();
        }

        public IList<Company> GetAll()
        {
            return _context.Companies.ToList();
        }

        public Contact GetContactOfCompany(int companyId)
        {
            return _context.Contacts.FirstOrDefault(c => c.CompanyId == companyId);
        }

        public List<Supervisor> GetSupervisorsOfCompany(int companyId)
        {
            return _context.Supervisors.Where(c => c.CompanyId == companyId).ToList();
        }

        public List<Student> GetStudentsOfCompany(int companyId)
        {
            var result = _context.Companies.Include(s => s.Supervisors).ThenInclude(s => s.Students).FirstOrDefault(c => c.CompanyId == companyId);
            if (result != null)
                return result.Supervisors.SelectMany(s => s.Students).ToList();
            else
                return null;
        }

        public void AddStudentWithSupervisorForCompany(Student student, Supervisor supervisor)
        {
            var studentFromDb = _context.Students.Find(student.Id);
            supervisor.Students.Add(student);
            studentFromDb.Supervisor = supervisor;
            _context.SaveChanges();
        }

        public void RemoveStudentFromSupervisor(Student student, Supervisor? supervisor)
        {
            if (student is not null)
            {
                var stud = _context.Students.Find(student.Id);
                if (stud is not null)
                {
                    stud.SupervisorId = null;
                    _context.SaveChanges();
                }
            }
        }
    }
}
