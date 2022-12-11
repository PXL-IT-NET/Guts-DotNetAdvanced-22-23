using InternshipsAdmin.AppLogic.Contracts;
using InternshipsAdmin.Domain;

namespace InternshipsAdmin.Infrastructure
{
    internal class StudentsRepository : IStudentsRepository
    {
        private InternshipsContext _context;
      
        public StudentsRepository(InternshipsContext context)
        {
            _context = context;
        }
        public ICollection<Student> GetAll()
        {
            throw new NotImplementedException();
        }

        public ICollection<Student> GetStudentsWithoutSupervisor()
        {
            throw new NotImplementedException();
        }
    }
}
