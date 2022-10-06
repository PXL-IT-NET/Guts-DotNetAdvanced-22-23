using System;
using System.Windows;

namespace Exercise2
{
    public partial class MainWindow : Window
    {
        public MainWindow(IStudentAdministration administration, IBlackBoard blackBoard)
        {
            InitializeComponent();

        }
        private void addStudentButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
