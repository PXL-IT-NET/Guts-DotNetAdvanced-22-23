using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Exercise2.Controls;
using Guts.Client.Core;
using Guts.Client.Core.TestTools;
using Guts.Client.WPF.TestTools;

namespace Exercise2.Tests
{
    [ExerciseTestFixture("dotNet2", "H01", "Exercise02", @"Exercise2\Controls\About.xaml;Exercise2\Controls\About.xaml.cs")]
    [Apartment(ApartmentState.STA)]
    public class AboutTests
    {
        private About _control;
        private Grid? _grid;
        private ScrollViewer? _scrollViewer;
        private TextBlock? _textBlock;


        [SetUp]
        public void BeforeEachTest()
        {
            _control = new About();

            _grid = _control.Content as Grid;
            if (_grid == null) return;

            _scrollViewer = _grid.FindVisualChildren<ScrollViewer>().FirstOrDefault();
            _textBlock = _scrollViewer.Content as TextBlock;
        }

        [MonitoredTest("About - Should not have changed the codebehind file")]
        public void _01_ShouldNotHaveChangedTheCodebehindFile()
        {
            var codeBehindFilePath = @"Exercise2\Controls\About.xaml.cs";
            string hash = Solution.Current.GetFileHash(codeBehindFilePath);
            Assert.That(hash, Is.EqualTo("9E-EF-25-E4-5F-FA-9C-C8-35-F6-2B-E1-73-CA-69-73"),
                $"The file '{codeBehindFilePath}' has changed. " +
                "Undo your changes on the file to make this test pass.");
        }

        [MonitoredTest("About - Should have all controls")]
        public void _02_ShouldHaveAllControls()
        {
            Assert.That(_grid, Is.Not.Null, "The Content of the control should be a Grid");
            Assert.That(_scrollViewer, Is.Not.Null, "The Grid should contain a ScrollViewer");
            Assert.That(_textBlock, Is.Not.Null, "The Content of the ScrollViewer should be a TextBlock that will contain the about text");
            Assert.That(HasMargin(_scrollViewer), Is.True, "The ScrollViewer should have some Margin on all sides");
            Assert.That(_scrollViewer.VerticalScrollBarVisibility, Is.EqualTo(ScrollBarVisibility.Auto),
                "The VerticalScrollBarVisibility of the ScrollViewer is not correct. " +
                "The scrollbar should only be visible when it is needed.");
        }

        [MonitoredTest("About - The Grid should have a linear gradient background")]
        public void _03_TheGridShouldHaveALinearGradientBackground()
        {
            _02_ShouldHaveAllControls();

            LinearGradientBrush? brush = _grid.Background as LinearGradientBrush;
            Assert.That(brush, Is.Not.Null,
                "The Background of the Grid is not a LinearGradientBrush. " +
                "Study this article first: https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.lineargradientbrush");
            Assert.That(brush.StartPoint == new Point(0, 0) && brush.EndPoint == new Point(1, 1), Is.True,
                "The gradient should flow from the top-left to the right bottom. " +
                "More info: https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.lineargradientbrush");
            Assert.That(brush.GradientStops.Count, Is.EqualTo(3), "The brush should have 3 GradientStops. " +
                                                                  "More info: https://learn.microsoft.com/en-us/dotnet/api/system.windows.media.lineargradientbrush");
            Assert.That(brush.GradientStops[0].Color == brush.GradientStops[2].Color, "The color of the first and last GradientStops should be the same");
            Assert.That(brush.GradientStops[0].Color != brush.GradientStops[1].Color, "The color of the first and second GradientStops should be different");
            Assert.That(brush.GradientStops[0].Offset == 0.0, Is.True, "The first color should start right at the beginning");
            Assert.That(brush.GradientStops[1].Offset == 0.5, Is.True, "The second color should be right in the middle");
            Assert.That(brush.GradientStops[2].Offset == 1.0, Is.True, "The last color should be right at the end");
        }

        [MonitoredTest("About - The TextBlock should display text at the bottom using Inlines")]
        public void _04_TheTextBlockShouldDisplayTextAtTheBottomUsingInlines()
        {
            _02_ShouldHaveAllControls();

            Assert.That(_textBlock.HorizontalAlignment, Is.EqualTo(HorizontalAlignment.Center),
                "The TextBlock should be displayed in the center of the horizontal available space");
            Assert.That(_textBlock.VerticalAlignment, Is.EqualTo(VerticalAlignment.Bottom),
                "The TextBlock should be displayed at the bottom of the vertical available space");
            Assert.That(_textBlock.TextWrapping, Is.EqualTo(TextWrapping.Wrap), "The TextBlock should wrap text that flows over the available space");
            InlineCollection inlines = _textBlock.Inlines;
            Assert.That(inlines.Count > 0 && string.IsNullOrEmpty(_textBlock.Text), Is.True,
                "The TextBlock should use its Inlines collection to display text (instead of the Text property). " +
                "Use Runs and LineBreaks to display different lines of text." +
                "See: https://learn.microsoft.com/en-us/dotnet/api/system.windows.documents.inlinecollection " +
                "and https://learn.microsoft.com/en-us/dotnet/api/system.windows.documents.inline");
            Assert.That(inlines.OfType<LineBreak>().Count(), Is.GreaterThanOrEqualTo(5),
                "There should be at least 5 LineBreaks in the Inlines collection of the TextBlock. " +
                "See https://learn.microsoft.com/en-us/dotnet/api/system.windows.documents.linebreak");
            Assert.That(inlines.OfType<Run>().Count(), Is.GreaterThanOrEqualTo(6),
                "There should be at least 6 Runs in the Inlines collection of the TextBlock. " +
                "See https://learn.microsoft.com/en-us/dotnet/api/system.windows.documents.run");
        }

        private bool HasMargin(FrameworkElement element)
        {
            return element.Margin.Left > 0 &&
                   element.Margin.Top > 0 &&
                   element.Margin.Right > 0 &&
                   element.Margin.Bottom > 0;
        }
    }
}