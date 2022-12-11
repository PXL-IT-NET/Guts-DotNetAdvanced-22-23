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
            throw new NotImplementedException();
        }

        public IList<Company> GetAll()
        {
            throw new NotImplementedException();
        }

        public Contact GetContactOfCompany(int companyId)
        {
            throw new NotImplementedException();
        }

        public List<Supervisor> GetSupervisorsOfCompany(int companyId)
        {
            throw new NotImplementedException();
        }

        public List<Student> GetStudentsOfCompany(int companyId)
        {
            throw new NotImplementedException();
        }

        public void AddStudentWithSupervisorForCompany(Student student, Supervisor supervisor)
        {
            throw new NotImplementedException();
        }

        public void RemoveStudentFromSupervisor(Student student, Supervisor? supervisor)
        {
            throw new NotImplementedException();

        }
    }
}
