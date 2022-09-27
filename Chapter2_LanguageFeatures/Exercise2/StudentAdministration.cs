using System.Collections.Generic;

namespace Exercise2
{
    public delegate void NewStudentEventHandler(object sender, StudentEventArgs e);
   
    public class StudentAdministration
    {
        private static StudentAdministration? _instance;
        private readonly IList<Student> _allStudents;

        public static StudentAdministration Instance => _instance ?? (_instance = new StudentAdministration());

        public event NewStudentEventHandler? NewStudentRegistered;

        private StudentAdministration()
        {
            _allStudents = new List<Student>();
        }

        public void RegisterStudent(Student student)
        {
            _allStudents.Add(student); //eventueel wat logica die controleert of de student al geregistreerd is?

            StudentEventArgs e = new StudentEventArgs(student);
            if (NewStudentRegistered != null)
            {
                NewStudentRegistered(this, e);
            }
        }
    }
}
