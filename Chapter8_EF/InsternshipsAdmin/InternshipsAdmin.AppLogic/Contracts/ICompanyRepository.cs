using InternshipsAdmin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipsAdmin.AppLogic.Contracts
{
    public interface ICompanyRepository
    {
        void Add(Company company);
        void AddStudentWithSupervisorForCompany(Student student, Supervisor supervisor);
        IList<Company> GetAll();
        List<Student> GetStudentsOfCompany(int companyId);
        List<Supervisor> GetSupervisorsOfCompany(int companyId);
        void RemoveStudentFromSupervisor(Student student, Supervisor? supervisor);
    }
}
