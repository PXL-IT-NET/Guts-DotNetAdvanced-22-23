using System.Windows;
using System.Windows.Controls;
using Exercise2.Controls;
using Exercise2.View;
using Exercise2.ViewModel;
using Guts.Client.WPF.TestTools;
using Moq;

namespace Exercise2.Tests;

[Apartment(ApartmentState.STA)]
[ExerciseTestFixture("dotnet2", "H03", "Exercise02",
    @"Exercise2\MainWindow.xaml;Exercise2\MainWindow.xaml.cs;")]
public class MainWindowTests
{
    private MainWindow _window;
    private HeaderControl? _header;
    private bool _resourcesLoaded;
    private IList<ContentControl> _contentControls;
    private Mock<IMainViewModel> _mainViewModelMock;
    private bool _viewModelLoadedWhenWindowIsLoaded;
    private bool _windowLoaded;
    private Task _loadCallBackTask;
    private ApplicationTester _applicationTester;

    [OneTimeSetUp]
    public void BeforeAllTests()
    {
        _viewModelLoadedWhenWindowIsLoaded = false;
        _mainViewModelMock = new Mock<IMainViewModel>();
        _mainViewModelMock.SetupGet(vm => vm.SideBarViewModel).Returns(new Mock<ISideBarViewModel>().Object);
        _mainViewModelMock.SetupGet(vm => vm.MovieDetailViewModel).Returns(new Mock<IMovieDetailViewModel>().Object);
        _mainViewModelMock.Setup(vm => vm.Load()).Callback(() =>
        {
            _loadCallBackTask = Task.Run(() =>
            {
                Thread.Sleep(50);
                if (_windowLoaded)
                {
                    _viewModelLoadedWhenWindowIsLoaded = true;
                }
            });
                
        });

        _applicationTester = new ApplicationTester();
        _applicationTester.LoadApplication(() =>
        {
            _window = new MainWindow(_mainViewModelMock.Object);
            _windowLoaded = false;
            _window.Loaded += (sender, e) =>
            {
                _windowLoaded = true;
            };
            _window.Show();

            _header = _window.FindVisualChildren<HeaderControl>().FirstOrDefault();
            _contentControls = _window.FindVisualChildren<ContentControl>().Where(cc => cc.GetType() == typeof(ContentControl)).ToList();
        });
    }

    [OneTimeTearDown]
    public void AfterAllTests()
    {
        _window?.Close();
    }

    [MonitoredTest("MainWindow - Should use HeaderControl")]
    public void _01_ShouldUseHeaderControl()
    {
        _applicationTester.AssertApplicationResourcesAreLoaded();

        Assert.That(_header, Is.Not.Null, "No HeaderControl found");
        int column = Grid.GetColumn(_header!);
        int row = Grid.GetRow(_header!);
        Assert.That(column == 0 && row == 0, Is.True, "Header is not positioned correctly in the Grid");
        int columnSpan = Grid.GetColumnSpan(_header!);
        int rowSpan = Grid.GetRowSpan(_header!);
        Assert.That(columnSpan == 2 && rowSpan == 1, Is.True, "Header is not correctly spanned in the Grid");
    }

    [MonitoredTest("MainWindow - Should use ContentControl for SideBar")]
    public void _02_ShouldUseContentControlForSideBar()
    {
        _applicationTester.AssertApplicationResourcesAreLoaded();

        Assert.That(_contentControls, Has.Count.GreaterThan(0), "No ContentControl found");

        ContentControl? sideBarContentControl = _contentControls.FirstOrDefault(cc =>
            cc.Content != null && cc.Content.GetType().IsAssignableTo(typeof(ISideBarViewModel)));
        Assert.That(sideBarContentControl, Is.Not.Null, "No ContentControl found that has an instance of 'ISideBarViewModel' as content");

        IList<DataTemplate> dataTemplates = _window.Resources.Values.OfType<DataTemplate>().ToList();
        DataTemplate? sideBarDataTemplate = dataTemplates.FirstOrDefault(dt => dt.DataType == typeof(SideBarViewModel));
        Assert.That(sideBarDataTemplate, Is.Not.Null, "There should be a DataTemplate defined for objects of type SideBarViewModel");
        SideBarView sideBarDataTemplateContent = sideBarDataTemplate.LoadContent() as SideBarView;
        Assert.That(sideBarDataTemplateContent, Is.Not.Null, "The DataTemplate for objects of type ISideBarViewModel should contain a SideBarView");

        int row = Grid.GetRow(sideBarContentControl!);
        int column = Grid.GetColumn(sideBarContentControl!);
        Assert.That(column == 0 && row == 1, Is.True, "SideBar is not positioned correctly in the Grid");
    }

    [MonitoredTest("MainWindow - Should use ContentControl for Movie details")]
    public void _03_ShouldUseContentControlForMovieDetails()
    {
        _applicationTester.AssertApplicationResourcesAreLoaded();

        Assert.That(_contentControls, Has.Count.GreaterThan(0), "No ContentControl found");

        ContentControl? movieDetailContentControl = _contentControls.FirstOrDefault(cc =>
            cc.Content != null && cc.Content.GetType().IsAssignableTo(typeof(IMovieDetailViewModel)));
        Assert.That(movieDetailContentControl, Is.Not.Null, "No ContentControl found that has an instance of 'IMovieDetailViewModel' as content");

        IList<DataTemplate> dataTemplates = _window.Resources.Values.OfType<DataTemplate>().ToList();
        DataTemplate? movieDetailDataTemplate = dataTemplates.FirstOrDefault(dt => dt.DataType == typeof(MovieDetailViewModel));
        Assert.That(movieDetailDataTemplate, Is.Not.Null, "There should be a DataTemplate defined for objects of type MovieDetailViewModel");
        MovieDetailView movieDetailDataTemplateContent = movieDetailDataTemplate.LoadContent() as MovieDetailView;
        Assert.That(movieDetailDataTemplateContent, Is.Not.Null, "The DataTemplate for objects of type MovieDetailViewModel should contain a MovieDetailView");

        int row = Grid.GetRow(movieDetailContentControl!);
        int column = Grid.GetColumn(movieDetailContentControl!);
        Assert.That(column == 1 && row == 1, Is.True, "Movie detail is not positioned correctly in the Grid");
    }

    [MonitoredTest("MainWindow - Should use a MainViewModel as the data binding source")]
    public void _04_ShouldUseAMainViewModelAsTheDataBindingSource()
    {
        _applicationTester.AssertApplicationResourcesAreLoaded();

        Assert.That(_window.DataContext, Is.SameAs(_mainViewModelMock.Object),
            "The injected IMainViewModel should be the data binding source for all bindings in the MainWindow");

        _loadCallBackTask?.Wait();

        Assert.That(_viewModelLoadedWhenWindowIsLoaded, Is.True,
            "The ViewModel is not loaded at all or the ViewModel is loaded before the window is loaded.");
    }
}