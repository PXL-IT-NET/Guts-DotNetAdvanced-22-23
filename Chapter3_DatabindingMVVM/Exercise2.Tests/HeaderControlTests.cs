using System.Windows.Controls;
using System.Windows.Media;
using Exercise2.Controls;
using Guts.Client.WPF.TestTools;

namespace Exercise2.Tests;

[Apartment(ApartmentState.STA)]
[ExerciseTestFixture("dotnet2", "H03", "Exercise02",
    @"Exercise2\Controls\HeaderControl.xaml;")]
public class HeaderControlTests
{
    private ApplicationTester _applicationTester;
    private HeaderControl _header;
    private Grid _grid;
    private TextBlock _textBlock;

    [OneTimeSetUp]
    public void BeforeAllTests()
    {
        _applicationTester = new ApplicationTester();

        _applicationTester.LoadApplication(() =>
        {
            _header = new HeaderControl();

            _grid = (_header.Content as Grid)!;
            _textBlock = _grid.FindVisualChildren<TextBlock>().First();
        });
    }

    [MonitoredTest("Header - Should have a dark radial background")]
    public void _01_ShouldHaveADarkRadialBackground()
    {
        RadialGradientBrush darkRadialGradientBrush = _applicationTester.TryGetApplicationResource<RadialGradientBrush>("DarkRadialBrush");
        Assert.That(_grid.Background, Is.SameAs(darkRadialGradientBrush));
    }

    [MonitoredTest("Header - Title - Should use the FontBrush in the application resources for the font color")]
    public void _02_Title_ShouldUseTheFontBrushInTheApplicationResourcesForTheFontColor()
    {
        var fontBrush = _applicationTester.TryGetApplicationResource<SolidColorBrush>("FontBrush");
        Assert.That(_textBlock.Foreground, Is.SameAs(fontBrush));
    }
}