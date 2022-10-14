using Exercise2.Model;
using Exercise2.ViewModel;

namespace Exercise2.Tests;

[ExerciseTestFixture("dotnet2", "H03", "Exercise02",
    @"Exercise2\ViewModel\IMovieDetailViewModel.cs;Exercise2\ViewModel\MovieDetailViewModel.cs;Exercise2\ViewModel\IViewModel.cs;Exercise2\ViewModel\ViewModelBase.cs;")]
public class MovieDetailViewModelTests
{
    private IMovieDetailViewModel? _viewModel;

    [SetUp]
    public void BeforeEachTest()
    {
        _viewModel = new MovieDetailViewModel() as IMovieDetailViewModel;
    }

    [MonitoredTest("MovieDetailViewModel - Should implement IMovieDetailViewModel and inherit from ViewModelBase")]
    public void _01_ShouldImplementIMovieDetailViewModelAndInheritFromViewModelBase()
    {
        Assert.That(_viewModel, Is.Not.Null, "IMovieDetailViewModel is not implemented");
        Assert.That(typeof(MovieDetailViewModel).IsAssignableTo(typeof(ViewModelBase)), "Does not inherit from ViewModelBase");
    }

    [MonitoredTest("IMovieDetailViewModel interface should not have been changed")]
    public void _02_IMovieDetailViewModelInterfaceShouldNotHaveBeenChanged()
    {
        string hash = Solution.Current.GetFileHash(@"Exercise2\ViewModel\IMovieDetailViewModel.cs");
        Assert.That(hash, Is.EqualTo("4C-B0-AD-9E-E8-F0-46-BE-02-97-17-E8-7F-3C-2C-BC"));
    }

    [MonitoredTest("MovieDetailViewModel - Movie - Should notify when it is changed")]
    public void _03_Movie_ShouldNotifyWhenItIsChanged()
    {
        _01_ShouldImplementIMovieDetailViewModelAndInheritFromViewModelBase();

        //Arrange
        bool moviePropertyChanged = false;
        _viewModel!.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(IMovieDetailViewModel.Movie))
            {
                moviePropertyChanged = true;
            }
        };

        //Act
        _viewModel.Movie = new Movie();

        //Assert
        Assert.That(moviePropertyChanged, Is.True);
    }

    [MonitoredTest("MovieDetailViewModel - Movie - Should notify that HasNoMovie could have changed")]
    public void _04_Movie_ShouldNotifyThatHasNoMovieCouldHaveChanged()
    {
        _01_ShouldImplementIMovieDetailViewModelAndInheritFromViewModelBase();

        //Arrange
        bool hasNoMoviePropertyChanged = false;
        _viewModel!.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(IMovieDetailViewModel.HasNoMovie))
            {
                hasNoMoviePropertyChanged = true;
            }
        };

        //Act
        _viewModel.Movie = new Movie();

        //Assert
        Assert.That(hasNoMoviePropertyChanged, Is.True);
    }

    [MonitoredTest("MovieDetailViewModel - HasNoMovie - Should return true when no Movie is set")]
    public void _05_HasNoMovieShouldReturnTrueWhenNoMovieIsSet()
    {
        _01_ShouldImplementIMovieDetailViewModelAndInheritFromViewModelBase();
        _viewModel!.Movie = null;
        Assert.That(_viewModel.HasNoMovie, Is.True);
        _viewModel!.Movie = new Movie();
        Assert.That(_viewModel.HasNoMovie, Is.False);
    }

    [MonitoredTest("MovieDetailViewModel - GiveFiveStarRatingCommand - Execute - Should set rating of Movie to 5")]
    public void _06_GiveFiveStarRatingCommand_Execute_ShouldSetRatingOfMovieTo5()
    {
        _01_ShouldImplementIMovieDetailViewModelAndInheritFromViewModelBase();
        _viewModel!.Movie = new Movie { Rating = 2 };
        Assert.That(_viewModel.GiveFiveStarRatingCommand, Is.Not.Null, "GiveFiveStarRatingCommand should not be null");

        //Act
        _viewModel.GiveFiveStarRatingCommand.Execute(null);

        //Assert
        Assert.That(_viewModel.Movie.Rating, Is.EqualTo(5));
    }

    [MonitoredTest("MovieDetailViewModel - GiveFiveStarRatingCommand - CanExecute - Should only be true if a Movie is set and the rating of that movie is not 5")]
    public void _07_GiveFiveStarRatingCommand_CanExecute_ShouldOnlyBeTrueIfAMovieIsSetAndTheRatingOfThatMovieIsNot5()
    {
        _01_ShouldImplementIMovieDetailViewModelAndInheritFromViewModelBase();
        Assert.That(_viewModel!.GiveFiveStarRatingCommand, Is.Not.Null, "GiveFiveStarRatingCommand should not be null");

        _viewModel.Movie = new Movie { Rating = 4 };
        Assert.That(_viewModel.GiveFiveStarRatingCommand.CanExecute(null), Is.True);
        _viewModel.Movie.Rating = 5;
        Assert.That(_viewModel.GiveFiveStarRatingCommand.CanExecute(null), Is.False);
        _viewModel.Movie = null;
        Assert.That(_viewModel.GiveFiveStarRatingCommand.CanExecute(null), Is.False);
    }

    [MonitoredTest("MovieDetailViewModel - GiveFiveStarRatingCommand - CanExecuteChanged - Should be raised when the movie changes")]
    public void _08_GiveFiveStarRatingCommand_CanExecuteChanged_ShouldBeRaisedWhenTheMovieChanges()
    {
        _01_ShouldImplementIMovieDetailViewModelAndInheritFromViewModelBase();
        Assert.That(_viewModel!.GiveFiveStarRatingCommand, Is.Not.Null, "GiveFiveStarRatingCommand should not be null");

        bool wasRaised = false;
        _viewModel.GiveFiveStarRatingCommand.CanExecuteChanged += (sender, e) =>
        {
            wasRaised = true;
        };

        _viewModel.Movie = new Movie { Rating = 1 };
        Assert.That(wasRaised, Is.True);

        wasRaised = false;

        _viewModel.Movie = null;
        Assert.That(wasRaised, Is.True);
    }

    [MonitoredTest("MovieDetailViewModel - GiveFiveStarRatingCommand - CanExecuteChanged - Should be raised when the rating of the movie changes")]
    public void _09_GiveFiveStarRatingCommand_CanExecuteChanged_ShouldBeRaisedWhenTheRatingOfTheMovieChanges()
    {
        _01_ShouldImplementIMovieDetailViewModelAndInheritFromViewModelBase();
        Assert.That(_viewModel!.GiveFiveStarRatingCommand, Is.Not.Null, "GiveFiveStarRatingCommand should not be null");

        bool wasRaised = false;
        _viewModel.GiveFiveStarRatingCommand.CanExecuteChanged += (sender, e) =>
        {
            wasRaised = true;
        };

        _viewModel.Movie = new Movie { Rating = 1 };
        Assert.That(wasRaised, Is.True);
        wasRaised = false;

        _viewModel.Movie.Rating = 5;
        Assert.That(wasRaised, Is.True);
    }
}