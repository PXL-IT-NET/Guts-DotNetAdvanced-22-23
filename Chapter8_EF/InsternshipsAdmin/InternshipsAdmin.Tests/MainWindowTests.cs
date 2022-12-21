using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Guts.Client.WPF.TestTools;
using InternshipsAdmin.AppLogic.Contracts;
using InternshipsAdmin.Domain;
using InternshipsAdmin.UI;
using Moq;
using System.Windows.Controls;

namespace InternshipsAdmin.Tests
{
    [ExerciseTestFixture("dotnet2", "H08", "Exercise01", @"InternshipsAdmin.UI\MainWindow.xaml.cs")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow _window = null;
        private Mock<ICompanyRepository> _companyRepositoryMock;
        private Mock<IStudentsRepository> _studentRepositoryMock;
        private List<Company> _allCompanies;
        private List<Student> _allStudentsOfCompany;
        private List<Student> _allStudentsWithoutSupervisor;
        private List<Student> _emptyStudentList;
        private List<Supervisor> _allSupervisors;
        private Grid _grid;
        private Button _addButton;
        private Button _removeButton;
        private ComboBox _studentsComboBox, _supervisorsComboBox;
        private Company _company;
        private DataGrid _companyDataGrid;
        private DataGrid _studentDataGrid;


        [SetUp]
        public void Setup()
        {

            _companyRepositoryMock = new Mock<ICompanyRepository>();
            _studentRepositoryMock = new Mock<IStudentsRepository>();
            _company = new Company(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            _company.CompanyId = 1;
            _allCompanies = new List<Company> { _company };
            _allSupervisors = new List<Supervisor> { new Supervisor(Guid.NewGuid().ToString()) };
            _allCompanies[0].Supervisors = _allSupervisors;
            _emptyStudentList = new List<Student> { };

            _allStudentsWithoutSupervisor = new List<Student> { new Student(Guid.NewGuid().ToString()) };
            _allStudentsOfCompany = new List<Student> { new Student(Guid.NewGuid().ToString()), new Student(Guid.NewGuid().ToString()) };

            _allSupervisors = new List<Supervisor> { new Supervisor(Guid.NewGuid().ToString()) };

            //setups
            _companyRepositoryMock.Setup(repo => repo.GetAll()).Returns(_allCompanies);
            _companyRepositoryMock.Setup(repo => repo.GetSupervisorsOfCompany(_company.CompanyId)).Returns(_allSupervisors);

            _studentRepositoryMock.Setup(repo => repo.GetStudentsWithoutSupervisor()).Returns(_allStudentsWithoutSupervisor);

            _companyRepositoryMock.Setup(repo => repo.GetStudentsOfCompany(_company.CompanyId)).Returns(_allStudentsOfCompany);

            _window = new MainWindow(_companyRepositoryMock.Object, _studentRepositoryMock.Object);
            _window.Show();

            _addButton = _window.GetPrivateFieldValue<Button>(f => f.Name == "AddStudentForCompanyButton");

            _studentsComboBox = _window.GetPrivateFieldValue<ComboBox>(f => f.Name == "StudentsComboBox");
            _supervisorsComboBox = _window.GetPrivateFieldValue<ComboBox>(f => f.Name == "SupervisorsComboBox");

            _companyDataGrid = _window.GetPrivateFieldValue<DataGrid>(f => f.Name == "CompanyDataGrid");
            _studentDataGrid = _window.GetPrivateFieldValue<DataGrid>(f => f.Name == "StudentDataGrid");

            _grid = _window.FindVisualChildren<Grid>().FirstOrDefault();
        }

        [MonitoredTest("MainWindow - Constructor should retrieve all companies and setup databindings")]
        public void _01_Constructor_ShouldRetrieveAllCompaniesAndSetupDatabinding()
        {
            _companyRepositoryMock.Verify(repos => repos.GetAll(), Times.Once,
                "The 'GetAll' method of the companyRepository should be used.");

            Assert.That(_companyDataGrid.ItemsSource, Is.EquivalentTo(_allCompanies),
                "The first datagrid should show the companies");
            Assert.That(_grid.RowDefinitions[2].Height.Value, Is.EqualTo(0), "The row with rowId 2 should not have a Height at startup (should not be visible)");
        }

        [MonitoredTest("MainWindow - DataGridSelectionChanged should Fill the comboboxex and update the students datagrid")]
        public void _02_DataGridSelectionChanged_ShouldFillTheComboboxesAndUpdateTheStudentsDataGrid()
        {
            _companyDataGrid.SelectedIndex = 0;

            Company company = (Company)_companyDataGrid.SelectedItem;
            _companyRepositoryMock.Setup(repo => repo.GetStudentsOfCompany(company.CompanyId)).Returns(_allStudentsOfCompany);

            _companyRepositoryMock.Verify(repos => repos.GetStudentsOfCompany(company.CompanyId), Times.Once,
"The 'GetStudentsWithoutSupervisor' method of the studentsRepository should be used.");
            
            Assert.That(_studentDataGrid.ItemsSource, Is.EquivalentTo(_allStudentsOfCompany),
    "The second datagrid should show the students of the company");

            Assert.That(_grid.RowDefinitions[2].Height.IsAuto, Is.True, "The row with rowId 2 should have an Auto Height");


            //update comboBoxes

            _studentRepositoryMock.Setup(repo => repo.GetStudentsWithoutSupervisor()).Returns(_allStudentsWithoutSupervisor);
            _companyRepositoryMock.Setup(repo => repo.GetSupervisorsOfCompany(company.CompanyId)).Returns(_allSupervisors);


            //comboboxes tests
            _studentRepositoryMock.Verify(repos => repos.GetStudentsWithoutSupervisor(), Times.Once,
    "The 'GetStudentsWithoutSupervisor' method of the studentsRepository should be used.");
            _companyRepositoryMock.Verify(repos => repos.GetSupervisorsOfCompany(It.IsAny<int>()), Times.Once,
    "The 'GetSupervisorsOfCompany' method of the companyRepository should be used.");

            
            Assert.That(_studentsComboBox.ItemsSource, Is.EquivalentTo(_allStudentsWithoutSupervisor),
                "The first combobox should show the students without a supervisor");

            Assert.That(_supervisorsComboBox.ItemsSource, Is.EquivalentTo(_allSupervisors),
                "The second combobox should show the Supervisors");

        }

        [MonitoredTest("MainWindow - AddStudentWithSupervisorButton should call companyrepository method")]
        public void _03_AddStudentWithSupervisorButton_ShouldCallCompanyRepositoryMethod()
        {
            _companyRepositoryMock.Setup(repo => repo.GetStudentsOfCompany(_company.CompanyId)).Returns(_allStudentsOfCompany);
            _companyDataGrid.SelectedIndex = 0;        

            _addButton.FireClickEvent();

            _companyRepositoryMock.Verify(repos => repos.AddStudentWithSupervisorForCompany(It.IsAny<Student>(), It.IsAny<Supervisor>()), Times.Once,
                "The 'AddStudentWithSupervisorForCompany' method of the companyRepository should be used.");
        }

        /*
        [Test]
        public void _04_RemoveStudentFromSupervisorButton_ShouldCallCompanyRepositoryMethod()
        {
            _companyDataGrid.SelectedIndex = 0;
            Company company = (Company)_companyDataGrid.SelectedItem;
            _companyRepositoryMock.Setup(repo => repo.GetStudentsOfCompany(company.CompanyId)).Returns(_allStudentsOfCompany);


            _studentDataGrid.SelectedIndex = 0;
            // var button = _window.GetPrivateFieldValue<Button>(f => f.Name == "RemoveStudentFromSupervisorButton");

            _removeButton = (Button)_studentDataGrid.FindVisualChildren<Button>().Single();


            _removeButton.FireClickEvent();

            _companyRepositoryMock.Verify(repos => repos.RemoveStudentFromSupervisor(It.IsAny<Student>(), It.IsAny<Supervisor>()), Times.Once,
                "The 'RemoveStudentWithSupervisorForCompany' method of the companyRepository should be used.");

        }
        */

        [TearDown]
        public void TearDown()
        {
            _window?.Close();
        }
    }
}
