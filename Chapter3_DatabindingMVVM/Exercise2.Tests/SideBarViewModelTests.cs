using System.Reflection;
using Exercise2.Data;
using Exercise2.Model;
using Exercise2.ViewModel;
using Moq;

namespace Exercise2.Tests;

[ExerciseTestFixture("dotnet2", "H03", "Exercise02",
    @"Exercise2\ViewModel\ISideBarViewModel.cs;Exercise2\ViewModel\SideBarViewModel.cs;Exercise2\ViewModel\IViewModel.cs;Exercise2\ViewModel\ViewModelBase.cs;")]
public class SideBarViewModelTests
{
    private Mock<IMovieRepository> _movieRepositoryMock;
    private ISideBarViewModel? _viewModel;

    [SetUp]
    public void BeforeEachTest()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _viewModel = new SideBarViewModel(_movieRepositoryMock.Object) as ISideBarViewModel;
    }

    [MonitoredTest("SideBarViewModel - Should implement ISideBarViewModel and inherit from ViewModelBase")]
    public void _01_ShouldImplementISideBarViewModelAndInheritFromViewModelBase()
    {
        Assert.That(_viewModel, Is.Not.Null, "ISideBarViewModel is not implemented");
        Assert.That(typeof(SideBarViewModel).IsAssignableTo(typeof(ViewModelBase)), "Does not inherit from ViewModelBase");
    }

    [MonitoredTest("ISideBarViewModel interface should not have been changed")]
    public void _02_ISideBarViewModelInterfaceShouldNotHaveBeenChanged()
    {
        string hash = Solution.Current.GetFileHash(@"Exercise2\ViewModel\ISideBarViewModel.cs");
        Assert.That(hash, Is.EqualTo("25-DF-4F-48-6F-4E-D4-9A-D7-8C-95-F3-91-98-A4-70"));
    }

    [MonitoredTest("SideBarViewModel - Constructor - Should make sure that Movies is not null")]
    public void _03_Constructor_ShouldMakeSureThatMoviesIsNotNull()
    {
        _01_ShouldImplementISideBarViewModelAndInheritFromViewModelBase();
        Assert.That(_viewModel!.Movies, Is.Not.Null);
        Assert.That(_viewModel.Movies.Count, Is.Zero, "After construction (and before load) the Movies list should be empty");
    }

    [MonitoredTest("SideBarViewModel - Load - Should retrieve all movies and set Movies property")]
    public void _04_Load_ShouldRetrieveAllMoviesAndSetMoviesProperty()
    {
        _01_ShouldImplementISideBarViewModelAndInheritFromViewModelBase();

        //Arrange
        IList<Movie> allMovies = new MovieRepository().GetAll();
        _movieRepositoryMock.Setup(repo => repo.GetAll()).Returns(allMovies);
        _movieRepositoryMock.Invocations.Clear();

        bool moviesPropertyChanged = false;
        _viewModel!.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(ISideBarViewModel.Movies))
            {
                moviesPropertyChanged = true;
            }
        };

        //Act
        _viewModel.Load();
        
        //Assert
        _movieRepositoryMock.Verify(repo => repo.GetAll(), Times.Once());
        Assert.That(_viewModel.Movies, Is.SameAs(allMovies),
            "The Movies property should contain the exact same instance that is returned by the repository");

        PropertyInfo moviesPropertyInfo = typeof(SideBarViewModel).GetProperty(nameof(ISideBarViewModel.Movies))!;
        bool moviesSetterIsPrivate = moviesPropertyInfo.GetSetMethod(true)?.IsPrivate ?? false;
        Assert.That(moviesSetterIsPrivate, Is.True, "The Movies property should have a private setter");

        Assert.That(moviesPropertyChanged, Is.True, "The Movies property should notify that it is changed");
    }

    [MonitoredTest("SideBarViewModel - SelectedMovie - Should notify when it is changed")]
    public void _05_SelectedMovie_ShouldNotifyWhenItIsChanged()
    {
        _01_ShouldImplementISideBarViewModelAndInheritFromViewModelBase(); 

        //Arrange
        bool selectedMoviePropertyChanged = false;
        _viewModel!.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(ISideBarViewModel.SelectedMovie))
            {
                selectedMoviePropertyChanged = true;
            }
        };

        //Act
        _viewModel.SelectedMovie = new Movie();

        //Assert
        Assert.That(selectedMoviePropertyChanged, Is.True);
    }
}