using System.ComponentModel;
using Exercise2.Model;
using Exercise2.ViewModel;
using Moq;

namespace Exercise2.Tests;

[ExerciseTestFixture("dotnet2", "H03", "Exercise02",
    @"Exercise2\ViewModel\IMainViewModel.cs;Exercise2\ViewModel\MainViewModel.cs;Exercise2\ViewModel\IViewModel.cs;Exercise2\ViewModel\ViewModelBase.cs;")]
public class MainViewModelTests
{
    private Mock<ISideBarViewModel> _sideBarViewModelMock;
    private Mock<IMovieDetailViewModel> _movieDetailViewModelMock;
    private IMainViewModel? _viewModel;

    [SetUp]
    public void BeforeEachTest()
    {
        _sideBarViewModelMock = new Mock<ISideBarViewModel>();
        _sideBarViewModelMock.SetupProperty(vm => vm.SelectedMovie);
        _movieDetailViewModelMock = new Mock<IMovieDetailViewModel>();
        _movieDetailViewModelMock.SetupProperty(vm => vm.Movie);
        _viewModel = new MainViewModel(_sideBarViewModelMock.Object, _movieDetailViewModelMock.Object) as IMainViewModel;
    }

    [MonitoredTest("MainViewModel - Should implement IMainViewModel and inherit from ViewModelBase")]
    public void _01_ShouldImplementIMainViewModelAndInheritFromViewModelBase()
    {
        Assert.That(_viewModel, Is.Not.Null, "IMainViewModel is not implemented");
        Assert.That(typeof(MainViewModel).IsAssignableTo(typeof(ViewModelBase)), "Does not inherit from ViewModelBase");
    }

    [MonitoredTest("IMainViewModel interface should not have been changed")]
    public void _02_IMainViewModelInterfaceShouldNotHaveBeenChanged()
    {
        string hash = Solution.Current.GetFileHash(@"Exercise2\ViewModel\IMainViewModel.cs");
        Assert.That(hash, Is.EqualTo("13-09-F1-FE-B5-22-47-07-2F-D2-12-03-5E-A2-FE-3D"));
    }

    [MonitoredTest("MainViewModel - Constructor - Should set sidebar and movie detail view models")]
    public void _03_Constructor_ShouldSetSideBarAndMovieDetailViewModels()
    {
        _01_ShouldImplementIMainViewModelAndInheritFromViewModelBase();
        Assert.That(_viewModel!.SideBarViewModel, Is.SameAs(_sideBarViewModelMock.Object));
        Assert.That(_viewModel.MovieDetailViewModel, Is.SameAs(_movieDetailViewModelMock.Object));
    }

    [MonitoredTest("MainViewModel - Load - Should load side bar")]
    public void _04_Load_ShouldLoadSideBar()
    {
        _01_ShouldImplementIMainViewModelAndInheritFromViewModelBase();
        _viewModel!.Load();
        _sideBarViewModelMock.Verify(vm => vm.Load(), Times.Once());
    }

    [MonitoredTest("MainViewModel - SelectedMovie property of SideBar changed - Should change Movie property of MovieDetail")]
    public void _05_SelectedMoviePropertyOfSideBarChanged_ShouldChangeMoviePropertyOfMovieDetail()
    {
        _01_ShouldImplementIMainViewModelAndInheritFromViewModelBase();
        var selectedMovie = new Movie();
        _sideBarViewModelMock.SetupGet(vm => vm.SelectedMovie).Returns(selectedMovie);
        _sideBarViewModelMock.Raise(vm => vm.PropertyChanged -= null, new PropertyChangedEventArgs(nameof(ISideBarViewModel.SelectedMovie)));
        Assert.That(_movieDetailViewModelMock.Object.Movie, Is.SameAs(selectedMovie));
    }
}