using System.Windows;

namespace Exercise2
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var administration = new StudentAdministration();
            var blackBoard = new BlackBoard();
            var mainWindow = new MainWindow(administration, blackBoard);
            mainWindow.Show();
        }
    }
}
