using System.Windows;
using Exercise1.FizzBuzz.AppLogic;

namespace Exercise1.FizzBuzz.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = new MainWindow(new FizzBuzzService());
            mainWindow.Show();
        }
    }
}
