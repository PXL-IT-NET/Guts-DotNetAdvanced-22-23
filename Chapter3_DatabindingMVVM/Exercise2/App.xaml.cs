using Exercise2.Data;
using Exercise2.ViewModel;
using System.Windows;

namespace Exercise2
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IMovieDetailViewModel movieDetailViewModel = new MovieDetailViewModel() as IMovieDetailViewModel;
            ISideBarViewModel sideBarViewModel = new SideBarViewModel(new MovieRepository()) as ISideBarViewModel;
            IMainViewModel mainViewModel = new MainViewModel(sideBarViewModel, movieDetailViewModel) as IMainViewModel;

            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}
