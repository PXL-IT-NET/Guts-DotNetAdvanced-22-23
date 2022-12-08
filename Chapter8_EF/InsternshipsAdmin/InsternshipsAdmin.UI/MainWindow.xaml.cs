using InternshipsAdmin.AppLogic.Contracts;
using InternshipsAdmin.Domain;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace InternshipsAdmin.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ICompanyRepository _companyReposistory;
        IStudentsRepository _studentsRepository;

        public MainWindow(ICompanyRepository companyRepository, IStudentsRepository studentsRepository)
        {
            InitializeComponent();
            _companyReposistory = companyRepository;
            _studentsRepository = studentsRepository;
            IList<Company> companies = _companyReposistory.GetAll();
            CompanyDataGrid.ItemsSource = companies;
            UpdateComboboxes();
            rowToHide.Height = new GridLength(0);

        }

        private void UpdateComboboxes()
        {
            StudentsComboBox.ItemsSource = _studentsRepository.GetStudentsWithoutSupervisor();
            StudentsComboBox.SelectedIndex = 0;
            Company company = (Company)CompanyDataGrid.SelectedItem;
            if (company != null)
            {
                SupervisorsComboBox.ItemsSource = _companyReposistory.GetSupervisorsOfCompany(company.CompanyId);
                SupervisorsComboBox.SelectedIndex = 0;
            }
        }

        private void CompanyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateStudentsDataGrid();
            UpdateComboboxes();
            rowToHide.Height = new GridLength(1, GridUnitType.Auto);
        }

        private void AddStudentForCompanyButton_Click(object sender, RoutedEventArgs e)
        {
            Student student = (Student)StudentsComboBox.SelectedItem;
            Supervisor supervisor = (Supervisor)SupervisorsComboBox.SelectedItem;
            _companyReposistory.AddStudentWithSupervisorForCompany(student, supervisor);
            StudentsComboBox.ItemsSource = _studentsRepository.GetStudentsWithoutSupervisor();
            UpdateStudentsDataGrid();
            UpdateComboboxes();
        }

        

        private void RemoveStudentFromSupervisorButton_Click(object sender, RoutedEventArgs e)
        {
            Student student = (Student)StudentDataGrid.SelectedItem;
            if (student.Supervisor != null)
            {
                Supervisor supervisor = student.Supervisor;
                _companyReposistory.RemoveStudentFromSupervisor(student, supervisor);
            }
            
            UpdateStudentsDataGrid();
            UpdateComboboxes();
           
        }

        private void UpdateStudentsDataGrid()
        {
            Company company = (Company)CompanyDataGrid.SelectedItem;
            ICollection<Student> students = _companyReposistory.GetStudentsOfCompany(company.CompanyId);
            StudentDataGrid.ItemsSource = students;
        }


    }
    
}
