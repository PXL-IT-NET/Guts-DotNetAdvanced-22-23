using InternshipsAdmin.Infrastructure;
using System.Windows;

namespace InternshipsAdmin.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var context = new InternshipsContext();
            var companyRepos = new CompanyRepository(context);
            var studentsRepos = new StudentsRepository(context);
            MainWindow window = new MainWindow(companyRepos, studentsRepos);
            window.Show();
        }
    }
}
