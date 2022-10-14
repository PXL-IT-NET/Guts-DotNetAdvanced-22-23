using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using Exercise2.Converters;
using Exercise2.View;
using Guts.Client.WPF.TestTools;
using TestTools;

namespace Exercise2.Tests;

[Apartment(ApartmentState.STA)]
[ExerciseTestFixture("dotnet2", "H03", "Exercise02",
    @"Exercise2\View\SideBarView.xaml;Exercise2\View\SideBarView.xaml.cs;")]
public class SideBarViewTests
{
    private SideBarView _view;
    private ApplicationTester _applicationTester;
    private Grid _grid;
    private ListView _listView;

    [OneTimeSetUp]
    public void BeforeAllTests()
    {
        _applicationTester = new ApplicationTester();
        _applicationTester.LoadApplication(() =>
        {
            _view = new SideBarView();
            _view.InitializeComponent();

            _grid = (_view.Content as Grid)!;
            _listView = _grid.FindVisualChildren<ListView>().First();
        });
    }

    [MonitoredTest("SideBarView - Should have a dark radial background")]
    public void _01_ShouldHaveADarkRadialBackground()
    {
        RadialGradientBrush darkRadialGradientBrush = _applicationTester.TryGetApplicationResource<RadialGradientBrush>("DarkRadialBrush");
        Assert.That(_grid.Background, Is.SameAs(darkRadialGradientBrush));
    }

    [MonitoredTest("SideBarView - ListView - Should be configured correctly")]
    public void _02_ListView_ShouldBeConfiguredCorrectly()
    {
        SolidColorBrush? background = _listView.Background as SolidColorBrush;
        Assert.That(background != null && background.Color == Colors.Transparent, Is.True,
            "ListView background should be transparent");

        BindingUtil.AssertBinding(_listView, ItemsControl.ItemsSourceProperty, "Movies", BindingMode.OneWay);
        BindingUtil.AssertBinding(_listView, Selector.SelectedItemProperty, "SelectedMovie", BindingMode.TwoWay);
    }

    [MonitoredTest("SideBarView - ListView ItemTemplate - Should be configured correctly")]
    public void _03_ListView_ItemTemplate_ShouldBeConfiguredCorrectly()
    {
        DataTemplate? dataTemplate = _listView.ItemTemplate;
        Assert.That(dataTemplate, Is.Not.Null, "No data template for items found");

        StackPanel? stackPanel = dataTemplate.LoadContent() as StackPanel;
        Assert.That(stackPanel, Is.Not.Null, "The root element of the data template should be a StackPanel");

        List<TextBlock> textBlocks = stackPanel.Children.OfType<TextBlock>().ToList();
        Assert.That(textBlocks.Count, Is.EqualTo(2),
            "There should be 2 TextBlocks in the StackPanel of the data template. " +
            "One for the title of the movie and one to display the rating");

        TextBlock titleTextBlock = textBlocks.First();
        BindingUtil.AssertBinding(titleTextBlock, TextBlock.TextProperty, "Title", BindingMode.OneWay, "Title TextBlock");
        SolidColorBrush fontBrush = _applicationTester.TryGetApplicationResource<SolidColorBrush>("FontBrush");
        Assert.That(titleTextBlock.Foreground, Is.SameAs(fontBrush), "The font color of the title TextBlock should be the 'FontBrush'");

        TextBlock ratingTextBlock = textBlocks.ElementAt(1);
        BindingExpression ratingBinding = BindingUtil.AssertBinding(ratingTextBlock, TextBlock.TextProperty, "Rating",
            BindingMode.OneWay, "Rating TextBlock");
        IValueConverter? converter = ratingBinding.ParentBinding.Converter;
        Assert.That(converter, Is.Not.Null, "A converter should be used for the rating");
        Assert.That(converter, Is.InstanceOf<RatingStarsConverter>(), "A RatingStarsConverter should be used for the rating");
    }
}