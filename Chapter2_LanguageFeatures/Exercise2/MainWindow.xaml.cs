using System.Windows;

namespace Exercise2
{
    public partial class MainWindow : Window
    {
        private BlackBoard _blackBoard;
        public MainWindow()
        {
            InitializeComponent();
            _blackBoard = new BlackBoard(blackBoardTextBlock);
        }
        private void addStudentButton_Click(object sender, RoutedEventArgs e)
        {
            Student student = new Student(
                firstNameTextBox.Text, 
                lastNameTextBox.Text,
                departmentTextBox.Text);

            StudentAdministration.Instance.RegisterStudent(student);
        }
    }
}
