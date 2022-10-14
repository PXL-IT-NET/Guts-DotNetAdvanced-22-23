namespace Exercise2.ViewModel;

public interface IMainViewModel : IViewModel
{
    ISideBarViewModel SideBarViewModel { get; }
    IMovieDetailViewModel MovieDetailViewModel { get; }
}