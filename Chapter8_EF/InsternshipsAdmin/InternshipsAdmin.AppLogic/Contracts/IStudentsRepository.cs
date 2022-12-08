using InternshipsAdmin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternshipsAdmin.AppLogic.Contracts
{
    public interface IStudentsRepository
    {
        ICollection<Student> GetAll();
        ICollection<Student> GetStudentsWithoutSupervisor();
    }
}
