namespace Exercise2;

public interface IStudentAdministration
{
    int StudentTotal { get; }

    void RegisterStudent(Student student);
}