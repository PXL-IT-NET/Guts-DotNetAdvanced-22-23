using System;
using System.Windows;

namespace Exercise2
{
    public partial class MainWindow : Window
    {
       // private readonly IStudentAdministration _administration;
        public MainWindow(IStudentAdministration administration, IBlackBoard blackBoard)
        {
            InitializeComponent();

            //_administration = administration;
            //blackBoard.SubscribeToStudentAdministrationEvents(administration, blackBoardTextBlock);
        }
        private void addStudentButton_Click(object sender, RoutedEventArgs e)
        {
            //Student student = new Student(
            //    firstNameTextBox.Text, 
            //    lastNameTextBox.Text,
            //    departmentTextBox.Text);

            //_administration.RegisterStudent(student);

            throw new NotImplementedException();
        }
    }
}
