using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Exercise2.Controls;
using Guts.Client.Core;
using Guts.Client.Core.TestTools;

namespace Exercise2.Tests;

[ExerciseTestFixture("dotNet2", "H01", "Exercise02", @"Exercise2\MainWindow.xaml;Exercise2\MainWindow.xaml.cs")]
[Apartment(ApartmentState.STA)]
public class MainWindowTests
{
    private MainWindow? _window;
    private TabControl? _tabControl;
    private TabItem? _chaptersTab;
    private TabItem? _aboutTab;

    [SetUp]
    public void BeforeEachTest()
    {
        _window = new MainWindow();
        _window.Show();

        _tabControl = _window.Content as TabControl;
        if (_tabControl == null) return;

        if (_tabControl.Items.Count > 0)
        {
            _chaptersTab = _tabControl.Items[0] as TabItem;
        }
        if (_tabControl.Items.Count > 1)
        {
            _aboutTab = _tabControl.Items[1] as TabItem;
        }
    }

    [TearDown]
    public void AfterEachTest()
    {
        _window?.Close();
    }

    [MonitoredTest("MainWindow - Should not have changed the codebehind file")]
    public void _01_ShouldNotHaveChangedTheCodebehindFile()
    {
        var codeBehindFilePath = @"Exercise2\MainWindow.xaml.cs";
        string hash = Solution.Current.GetFileHash(codeBehindFilePath);
        Assert.That(hash, Is.EqualTo("45-87-48-FC-AD-0E-8A-18-73-4F-C9-8C-27-6A-7A-D3"),
            $"The file '{codeBehindFilePath}' has changed. " +
            "Undo your changes on the file to make this test pass.");
    }

    [MonitoredTest("MainWindow - Should have 2 tabs")]
    public void _02_ShouldHave2Tabs()
    {
        Assert.That(_tabControl, Is.Not.Null, "The Content of the Window should be a TabControl");
        Assert.That(_chaptersTab, Is.Not.Null, "The TabItem for the chapters is not found");
        Assert.That(_aboutTab, Is.Not.Null, "The 'about' TabItem is not found");
    }

    [MonitoredTest("MainWindow - Chapter tab should be set correctly")]
    public void _03_ChapterTab_ShouldBeSetCorrectly()
    {
        _02_ShouldHave2Tabs();

        StackPanel? headerStackPanel = _chaptersTab.Header as StackPanel;
        Assert.That(headerStackPanel, Is.Not.Null, "The value of the tab Header should be a StackPanel");
        Assert.That(headerStackPanel.Orientation, Is.EqualTo(Orientation.Horizontal),
            "The header StackPanel should display its children horizontally");
        Assert.That(headerStackPanel.Children.Count, Is.EqualTo(2), "The header StackPanel should contain 2 children");
        Ellipse? ellipse = headerStackPanel.Children.OfType<Ellipse>().FirstOrDefault();
        Assert.That(ellipse, Is.Not.Null, "There should be an Ellipse in the header StackPanel");
        Assert.That(ellipse.Height > 0, Is.True, "The Ellipse in the header should have a fixed Height");
        Assert.That(ellipse.Fill, Is.InstanceOf<SolidColorBrush>(),
            "The Fill of the Ellipse in the header should be a solid color");
        TextBlock? textBlock = headerStackPanel.Children.OfType<TextBlock>().FirstOrDefault();
        Assert.That(textBlock, Is.Not.Null, "There should be a TextBlock in the header StackPanel");
        Assert.That(string.IsNullOrEmpty(textBlock.Text), Is.False, "The TextBlock in the header should have some text");

        Chapters? chaptersControl = _chaptersTab.Content as Chapters;
        Assert.That(chaptersControl, Is.Not.Null,
            "The content of the tab should be an instance of the Chapters UserControl");
    }

    [MonitoredTest("MainWindow - About tab should be set correctly")]
    public void _04_AboutTab_ShouldBeSetCorrectly()
    {
        _02_ShouldHave2Tabs();

        Assert.That(_aboutTab.Header is string headerText && !string.IsNullOrEmpty(headerText), Is.True,
            "The Header of the about tab should be a non-empty string");

        About? aboutControl = _aboutTab.Content as About;
        Assert.That(aboutControl, Is.Not.Null,
            "The content of the tab should be an instance of the About UserControl");
    }
}