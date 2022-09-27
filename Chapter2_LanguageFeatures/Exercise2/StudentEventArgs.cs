using System;

namespace Exercise2
{
    public class StudentEventArgs : EventArgs
    {
        public StudentEventArgs(Student student)
        {
            Student = student;
        }
        public Student Student { get; set; }
    }
}