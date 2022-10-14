using Guts.Client.WPF.TestTools;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TestTools;


namespace Exercise1.Tests
{
    [ExerciseTestFixture("dotnet2", "H03", "Exercise01",
        @"Exercise1\MainWindow.xaml;Exercise1\MainWindow.xaml.cs;")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow _window;
        private ListView? _listView;
        private GroupBox _newMovieGroupBox;
        private Label _titleLabel;
        private TextBox _titleTextBox;
        private Label _directorLabel;
        private TextBox _directorTextBox;
        private Label _releaseYearLabel;
        private TextBox _releaseYearTextBox;
        private Button _addNewMovieButton;
        private TextBlock _errorMessageTextBlock;

        [SetUp]
        public void Setup()
        {
            _window = new MainWindow();
            _window.Show();

            _listView = _window.FindVisualChildren<ListView>().FirstOrDefault();

            _newMovieGroupBox = _window.GetPrivateFieldValue<GroupBox>(f => f.Name == "NewMovieGroupBox");
            _titleLabel = _window.GetPrivateFieldValue<Label>(f => f.Name == "TitleLabel");
            _titleTextBox = _window.GetPrivateFieldValue<TextBox>(f => f.Name == "TitleTextBox");
            _directorLabel = _window.GetPrivateFieldValue<Label>(f => f.Name == "DirectorLabel");
            _directorTextBox = _window.GetPrivateFieldValue<TextBox>(f => f.Name == "DirectorTextBox");
            _releaseYearLabel = _window.GetPrivateFieldValue<Label>(f => f.Name == "ReleaseYearLabel");
            _releaseYearTextBox = _window.GetPrivateFieldValue<TextBox>(f => f.Name == "ReleaseYearTextBox");
            _addNewMovieButton = _window.GetPrivateFieldValue<Button>(f => f.Name == "AddNewMovieButton");
            _errorMessageTextBlock = _window.GetPrivateFieldValue<TextBlock>(f => f.Name == "ErrorMessageTextBlock");
        }

        [TearDown]
        public void TearDown()
        {
            _window.Close();
        }

        [MonitoredTest("Should not have changed Movie.cs"), Order(1)]
        public void _01_ShouldNotHaveChangedMovieClass()
        {
            var hash = Solution.Current.GetFileHash(@"Exercise1\Movie.cs");

            Assert.That(hash, Is.EqualTo("08-67-98-5C-EC-F3-AD-F8-F9-EF-DC-33-33-41-FF-5D"));
        }

        [MonitoredTest("The ListView source of data should be a collection that notifies of changes"), Order(2)]
        public void _02_ListView_SourceOfData_ShouldBeACollectionThatNotifiesOfChanges()
        {
            AssertHasListView();

            var collectionThatNotifiesChanges = _listView.ItemsSource as INotifyCollectionChanged;
            Assert.That(collectionThatNotifiesChanges, Is.Not.Null,
                "The source of data is not set or isn't of a type that notifies changes.");

            Assert.That(collectionThatNotifiesChanges, Is.InstanceOf<ObservableCollection<Movie>>(),
                "The 'ObservableCollection' class implements 'INotifyCollectionChanged'. " +
                "There is no need use an own implementation.");

            List<Movie> movies = _listView.ItemsSource.OfType<Movie>().ToList();
            Assert.That(movies, Has.Count.GreaterThanOrEqualTo(2),
                "The source of data should contain at least 2 instances of 'Movie'.");
        }

        [MonitoredTest("The ListView should have 3 columns"), Order(3)]
        public void _03_ListView_ShouldHave3Columns()
        {
            GridViewColumnCollection columns = AssertAndGetListViewColumns();

            string columnHeaderErrorMessage = "One of the column headers is not correct.";
            Assert.That(columns,
                Has.One.Matches((GridViewColumn column) => (column.Header as string) == "Title"),
                columnHeaderErrorMessage);
            Assert.That(columns,
                Has.One.Matches((GridViewColumn column) => (column.Header as string) == "Director"),
                columnHeaderErrorMessage);
            Assert.That(columns,
                Has.One.Matches((GridViewColumn column) => (column.Header as string) == "Release year"),
                columnHeaderErrorMessage);
        }

        [MonitoredTest("The ListView columns should have correct bindings defined"), Order(4)]
        public void _04_ListView_Columns_ShouldHaveCorrectBindings()
        {
            GridViewColumnCollection columns = AssertAndGetListViewColumns();

            GridViewColumn titleColumn = columns.First();
            AssertDisplayMemberBinding(titleColumn, "Title");

            GridViewColumn directorColumn = columns.ElementAt(1);
            AssertDisplayMemberBinding(directorColumn, "Director");

            GridViewColumn releaseYearColumn = columns.ElementAt(2);
            AssertDisplayMemberBinding(releaseYearColumn, "ReleaseYear");
        }

        [MonitoredTest("The movie form should have correct bindings"), Order(5)]
        public void _05_MovieForm_ShouldHaveCorrectBindings()
        {
            AssertHasFormControls();

            BindingUtil.AssertBinding(_titleTextBox, TextBox.TextProperty, "Title", BindingMode.TwoWay);
            BindingUtil.AssertBinding(_directorTextBox, TextBox.TextProperty, "Director", BindingMode.TwoWay);
            BindingUtil.AssertBinding(_releaseYearTextBox, TextBox.TextProperty, "ReleaseYear", BindingMode.TwoWay);
        }

        [MonitoredTest("The movie form should be bound to an unknown movie"), Order(6)]
        public void _06_MovieForm_ShouldBeBoundToAnUnknownMovie()
        {
            AssertHasFormControls();

            Movie? movie = _newMovieGroupBox.DataContext as Movie;
            Assert.That(movie, Is.Not.Null, "No movie 'DataContext' found for the 'GroupBox'.");

            var mustBeUnknownMovieMessage = "The movie should be unknown (unknown title, unknown director, release year 0).";
            var emptyMovie = new Movie();
            Assert.That(movie!.Title, Is.EqualTo(emptyMovie.Title), mustBeUnknownMovieMessage);
            Assert.That(movie.Director, Is.EqualTo(emptyMovie.Director), mustBeUnknownMovieMessage);
            Assert.That(movie.ReleaseYear, Is.EqualTo(emptyMovie.ReleaseYear), mustBeUnknownMovieMessage);
        }

        [MonitoredTest("Add movie should show an error message for an invalid movie"), Order(7)]
        public void _07_MovieForm_AddMovie_ShouldShowErrorMessageForInvalidMovie()
        {
            bool movieWasAdded = TryAddMovie("Unknown", Guid.NewGuid().ToString(), 1980, out _);
            Assert.That(movieWasAdded, Is.False, "A movie with title 'Unknown' can be added.");
            Assert.That(_errorMessageTextBlock.Text, Is.Not.Empty,
                "No error message is shown when adding a movie with an 'Unknown' title.");

            movieWasAdded = TryAddMovie(Guid.NewGuid().ToString(), "Unknown", 1980, out _);
            Assert.That(movieWasAdded, Is.False, "A movie with director 'Unknown' can be added.");
            Assert.That(_errorMessageTextBlock.Text, Is.Not.Empty,
                "No error message is shown when adding a movie with an 'Unknown' director.");

            movieWasAdded = TryAddMovie(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 0, out _);
            Assert.That(movieWasAdded, Is.False, "A movie with releaseYear <= 0 can be added.");
            Assert.That(_errorMessageTextBlock.Text, Is.Not.Empty,
                "No error message is shown when adding a movie with release year 0.");
        }

        [MonitoredTest("Add movie should add a valid movie to the movies collection"), Order(8)]
        public void _08_MovieForm_AddMovie_ShouldAddValidMovieToTheMoviesCollection()
        {
            AssertHasFormControls();

            var movieToAdd = new Movie
            {
                Title = Guid.NewGuid().ToString(),
                Director = Guid.NewGuid().ToString(),
                ReleaseYear = 2000
            };

            bool movieWasAdded = TryAddMovie(movieToAdd.Title, movieToAdd.Director, movieToAdd.ReleaseYear, out Movie addedMovie);
            string failMessage = "A valid movie is not added correctly.";
            Assert.That(movieWasAdded, Is.True, failMessage);
            Assert.That(addedMovie.Title, Is.EqualTo(movieToAdd.Title), failMessage);
            Assert.That(addedMovie.Director, Is.EqualTo(movieToAdd.Director), failMessage);
            Assert.That(addedMovie.ReleaseYear, Is.EqualTo(movieToAdd.ReleaseYear), failMessage);
        }

        [MonitoredTest("The movie form should be bound to a new unknown movie after adding one"), Order(9)]
        public void _09_MovieForm_ShouldBeBoundToANewUnknownMovieAfterAddingOne()
        {
            var movieToAdd = new Movie
            {
                Title = Guid.NewGuid().ToString(),
                Director = Guid.NewGuid().ToString(),
                ReleaseYear = 2000
            };

            bool movieWasAdded = TryAddMovie(movieToAdd.Title, movieToAdd.Director, movieToAdd.ReleaseYear, out Movie addedMovie);
            Assert.That(movieWasAdded, Is.True, "A valid movie is not added correctly.");

            Movie? nextNewMovie = _newMovieGroupBox.DataContext as Movie;
            var failMessage = "The 'DataContext' of the 'GroupBox' is not set to a new unknown movie.";
            Assert.That(nextNewMovie, Is.Not.Null, failMessage);
            Assert.That(nextNewMovie, Is.Not.SameAs(addedMovie), failMessage);
            Assert.That(nextNewMovie!.Title, Is.EqualTo("Unknown"), failMessage);
            Assert.That(nextNewMovie.Director, Is.EqualTo("Unknown"), failMessage);
            Assert.That(nextNewMovie.ReleaseYear, Is.Zero, failMessage);
        }

        [MonitoredTest("The movie form should clear previous error message after adding a movie"), Order(10)]
        public void _10_MovieForm_ShouldClearPreviousErrorMessageAfterAddingAMovie()
        {
            AssertHasFormControls();

            _errorMessageTextBlock.Text = "A previous error message";
            _08_MovieForm_AddMovie_ShouldAddValidMovieToTheMoviesCollection();
            Assert.That(_errorMessageTextBlock.Text, Is.Null.Or.Empty,
                "Previous error messages are not cleared when the add operation succeeds.");
        }

        [MonitoredTest("The movie form labels should have correct targets"), Order(5)]
        public void _11_MovieForm_LabelsShouldHaveCorrectTargets()
        {
            AssertHasFormControls();
           
            BindingUtil.AssertElementBinding(_titleLabel, Label.TargetProperty, BindingMode.OneWay, _titleTextBox);
            BindingUtil.AssertElementBinding(_directorLabel, Label.TargetProperty, BindingMode.OneWay, _directorTextBox);
            BindingUtil.AssertElementBinding(_releaseYearLabel, Label.TargetProperty, BindingMode.OneWay, _releaseYearTextBox);
        }

        private void AssertHasFormControls()
        {
            Assert.That(_newMovieGroupBox, Is.Not.Null, "Could not find the 'GroupBox' with title 'NewMovieGroupBox'.");
            Assert.That(_titleTextBox, Is.Not.Null, "Could not find the 'TextBox' with title 'NameTextBox'.");
            Assert.That(_directorTextBox, Is.Not.Null, "Could not find the 'TextBox' with title 'DescriptionTextBox'.");
            Assert.That(_addNewMovieButton, Is.Not.Null, "Could not find the 'Button' with title 'AddNewMovieButton'.");
            Assert.That(_errorMessageTextBlock, Is.Not.Null, "Could not find the 'TextBlock' with title 'ErrorMessageTextBlock'.");
        }

        private void AssertHasListView()
        {
            Assert.That(_listView, Is.Not.Null, "Cannot find a 'ListView'. Have you deleted it?");
        }

        private GridViewColumnCollection AssertAndGetListViewColumns()
        {
            AssertHasListView();

            var gridView = _listView!.View as GridView;
            Assert.That(gridView, Is.Not.Null, "The 'View' property of the 'ListView' should be an instance of the 'GridView' class.");
            Assert.That(gridView!.Columns, Has.Count.EqualTo(3), "The columns of the 'GridView' are not defined correctly.");
            return gridView.Columns;
        }

        private bool TryAddMovie(string title, string director, int releaseYear, out Movie addedMovie)
        {
            addedMovie = new Movie();
            AssertHasFormControls();
            AssertHasListView();

            AssertDoesNotReadTextBoxPropertiesInClickEventHandler();

            var originalMovies = GetMoviesFromListView();
            Movie? movie = _newMovieGroupBox.DataContext as Movie;
            Assert.That(movie, Is.Not.Null, "No movie 'DataContext' found for the 'GroupBox'.");
            movie!.Title = title;
            movie.Director = director;
            movie.ReleaseYear = releaseYear;
            _addNewMovieButton.FireClickEvent();

            var movies = GetMoviesFromListView();
            if (movies.Count == originalMovies.Count + 1)
            {
                addedMovie = movies.Last();
                return true;
            }

            return false;
        }

        private IList<Movie> GetMoviesFromListView()
        {
            var movies = new List<Movie>();
            if (_listView!.ItemsSource != null)
            {
                movies = _listView.ItemsSource.OfType<Movie>().ToList();
            }
            return movies;
        }

        private void AssertDoesNotReadTextBoxPropertiesInClickEventHandler()
        {
            var code = Solution.Current.GetFileContent(@"Exercise1\MainWindow.xaml.cs");
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = syntaxTree.GetRoot();
            var clickEventHandlerMethods = root
                .DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Where(md =>
                {
                    var parameters = md.ParameterList.Parameters;
                    if (parameters.Count != 2) return false;
                    if (!(parameters[0] is ParameterSyntax senderParameter)) return false;
                    if (senderParameter.Type?.ToString().ToLower() != "object")
                    {
                        return false;
                    }

                    if (!(parameters[1] is ParameterSyntax eventArgsParameter)) return false;
                    if (eventArgsParameter.Type?.ToString() != "RoutedEventArgs")
                    {
                        return false;
                    }

                    return true;
                }).ToList();

            Assert.That(clickEventHandlerMethods, Has.Count.LessThanOrEqualTo(1),
                "Your code contains more than one click event handler.");

            Assert.That(clickEventHandlerMethods, Has.Count.EqualTo(1), "Cannot find a click event handler.");

            var addNewMovieButtonClickMethod = clickEventHandlerMethods.First();

            var textPropertyReads = addNewMovieButtonClickMethod.Body.DescendantNodes()
                .OfType<MemberAccessExpressionSyntax>()
                .Where(ma => ma.ToString().ToLower().Contains("textbox") && ma.Name.ToString() == "Text" && !(ma.Parent is AssignmentExpressionSyntax)).ToList();

            Assert.That(textPropertyReads, Is.Empty,
                "You should not read the Text property of the title, director or release year TextBox. " +
                "Since you are using two-way data binding you can access the data source object directly.");
        }

        private void AssertDisplayMemberBinding(GridViewColumn column, string expectedPath)
        {
            var columnBinding = column.DisplayMemberBinding as Binding;
            var invalidNameBindingMessage = $"The cells in the '{expectedPath}' column are not correctly bound to the movies.";
            Assert.That(columnBinding, Is.Not.Null, invalidNameBindingMessage);
            Assert.That(columnBinding.Path.Path, Is.EqualTo(expectedPath), invalidNameBindingMessage);
            Assert.That(columnBinding.Mode, Is.AnyOf(BindingMode.Default, BindingMode.OneWay), invalidNameBindingMessage);
        }
    }
}