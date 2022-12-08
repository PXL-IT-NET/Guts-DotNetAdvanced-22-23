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
            return _context.Students.ToList();
        }

        public ICollection<Student> GetStudentsWithoutSupervisor()
        {
            return _context.Students.Where(s=>s.SupervisorId == null).ToList();
        }
    }
}
