using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using Exercise2.View;
using TestTools;

namespace Exercise2.Tests;

[Apartment(ApartmentState.STA)]
[ExerciseTestFixture("dotnet2", "H03", "Exercise02",
    @"Exercise2\View\MovieDetailView.xaml;Exercise2\View\MovieDetailView.xaml.cs;")]
public class MovieDetailViewTests
{
    private MovieDetailView _view;
    private ApplicationTester _applicationTester;
    private Grid _outerGrid;
    private ScrollViewer _scrollViewer;
    private Grid _noMovieGrid;
    private Grid _detailGrid;

    [OneTimeSetUp]
    public void BeforeAllTests()
    {
        _applicationTester = new ApplicationTester();
        _applicationTester.LoadApplication(() =>
        {
            _view = new MovieDetailView();
            _view.InitializeComponent();

            _outerGrid = (_view.Content as Grid)!;
            _scrollViewer = _outerGrid.Children.OfType<ScrollViewer>().First();
            _detailGrid = (_scrollViewer.Content as Grid)!;
            _noMovieGrid = _outerGrid.Children.OfType<Grid>().First();
        });
    }

    [MonitoredTest("MovieDetailView - ScrollViewer -  Should have a dark solid background")]
    public void _01_ScrollViewer_ShouldHaveADarkSolidBackground()
    {
        SolidColorBrush darkSolidBrush = _applicationTester.TryGetApplicationResource<SolidColorBrush>("DarkSolidBrush");
        Assert.That(_scrollViewer.Background, Is.SameAs(darkSolidBrush));
    }

    [MonitoredTest("MovieDetailView - Title - Should be configured correctly")]
    public void _02_Title_ShouldBeConfiguredCorrectly()
    {
        TextBlock titleTextBlock = _detailGrid.Children.OfType<TextBlock>().First();

        SolidColorBrush fontBrush = _applicationTester.TryGetApplicationResource<SolidColorBrush>("FontBrush");
        Assert.That(titleTextBlock.Foreground, Is.SameAs(fontBrush), "Use the 'FontBrush' resource for the color of the title");

        BindingUtil.AssertBinding(titleTextBlock, TextBlock.TextProperty, "Movie.Title", BindingMode.OneWay);

        Assert.That(titleTextBlock.HorizontalAlignment, Is.EqualTo(HorizontalAlignment.Center), "The title should be centered horizontally");
        Assert.That(titleTextBlock.FontSize, Is.GreaterThan(15), "The font size of the title should be bigger");
    }

    [MonitoredTest("MovieDetailView - Opening crawl - Should be configured correctly")]
    public void _03_OpeningCrawl_ShouldBeConfiguredCorrectly()
    {
        TextBlock crawlTextBlock = _detailGrid.Children.OfType<TextBlock>().ElementAt(1);

        SolidColorBrush fontBrush = _applicationTester.TryGetApplicationResource<SolidColorBrush>("FontBrush");
        Assert.That(crawlTextBlock.Foreground, Is.SameAs(fontBrush), "Use the 'FontBrush' resource for the color of the opening crawl");

        BindingUtil.AssertBinding(crawlTextBlock, TextBlock.TextProperty, "Movie.OpeningCrawl", BindingMode.OneWay);

        Assert.That(crawlTextBlock.HorizontalAlignment, Is.EqualTo(HorizontalAlignment.Center), "The opening crawl should be centered horizontally");
        Assert.That(crawlTextBlock.FontStyle, Is.EqualTo(FontStyles.Italic), "The opening crawl should be italic");
    }

    [MonitoredTest("MovieDetailView - Rating - Should be configured correctly")]
    public void _04_Rating_ShouldBeConfiguredCorrectly()
    {
        StackPanel ratingStackPanel = _detailGrid.Children.OfType<StackPanel>().First();
        Assert.That(ratingStackPanel.HorizontalAlignment, Is.EqualTo(HorizontalAlignment.Center), "The rating StackPanel should be centered horizontally");

        TextBlock ratingTextBlock = ratingStackPanel.Children.OfType<TextBlock>().First();
        SolidColorBrush fontBrush = _applicationTester.TryGetApplicationResource<SolidColorBrush>("FontBrush");
        Assert.That(ratingTextBlock.Foreground, Is.SameAs(fontBrush), "Use the 'FontBrush' resource for the color of the rating TextBlock");

        Slider ratingSlider = ratingStackPanel.Children.OfType<Slider>().First();
        Assert.That(ratingSlider.Foreground, Is.SameAs(fontBrush),
            "Use the 'FontBrush' resource for the color of the rating Slider");
        Assert.That(ratingSlider.Minimum == 1 && ratingSlider.Maximum == 5, Is.True,
            "Make sure the slider can only contain values between 1 and 5");
        Assert.That(ratingSlider.TickPlacement, Is.EqualTo(TickPlacement.BottomRight),
            "The slider should display Ticks at the BottomRight");
        Assert.That(ratingSlider.AutoToolTipPlacement, Is.EqualTo(AutoToolTipPlacement.TopLeft),
            "The slider should display tooltips for the Ticks at the TopLeft");
        BindingUtil.AssertBinding(ratingSlider, RangeBase.ValueProperty, "Movie.Rating", BindingMode.TwoWay);

        Button ratingButton = ratingStackPanel.Children.OfType<Button>().First();
        BindingUtil.AssertBinding(ratingButton, ButtonBase.CommandProperty, "GiveFiveStarRatingCommand", BindingMode.OneWay);
    }

    [MonitoredTest("MovieDetailView - No movie selected - Should show 'No movie' Grid on top")]
    public void _05_NoMovieSelected_ShouldShowNoMovieGridOnTop()
    {
        SolidColorBrush darkSolidBrush = _applicationTester.TryGetApplicationResource<SolidColorBrush>("DarkSolidBrush");
        Assert.That(_noMovieGrid.Background, Is.SameAs(darkSolidBrush), "The 'No Movie' Grid should have the dark solid background.");

        TextBlock textBlock = _noMovieGrid.Children.OfType<TextBlock>().First();
        SolidColorBrush fontBrush = _applicationTester.TryGetApplicationResource<SolidColorBrush>("FontBrush");
        Assert.That(textBlock.Foreground, Is.SameAs(fontBrush), "Use the 'FontBrush' resource for the color of the TextBlock");

        BindingExpression visibilityBinding = BindingUtil.AssertBinding(_noMovieGrid, UIElement.VisibilityProperty,
            "HasNoMovie", BindingMode.OneWay);
        Assert.That(visibilityBinding.ParentBinding.Converter, Is.InstanceOf<BooleanToVisibilityConverter>(),
            "The binding for the Visibility of the Grid should use the BooleanToVisibility converter");
    }
}