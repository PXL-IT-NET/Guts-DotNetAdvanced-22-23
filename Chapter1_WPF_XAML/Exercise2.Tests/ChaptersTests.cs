using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Exercise2.Controls;
using Guts.Client.Core;
using Guts.Client.Core.TestTools;

namespace Exercise2.Tests
{
    [ExerciseTestFixture("dotNet2", "H01", "Exercise02", @"Exercise2\Controls\Chapters.xaml;Exercise2\Controls\Chapters.xaml.cs")]
    [Apartment(ApartmentState.STA)]
    public class ChaptersTests
    {
        private Chapters? _control;
        private DockPanel? _dockPanel;
        private List<TextBlock> _textBlocks;
        private List<Rectangle> _rectangles;
        private ListView? _listView;

        [SetUp]
        public void BeforeEachTest()
        {
            _control = new Chapters();

            _dockPanel = _control.Content as DockPanel;
            if (_dockPanel == null) return;

            _textBlocks = _dockPanel.Children.OfType<TextBlock>().ToList();
            _rectangles = _dockPanel.Children.OfType<Rectangle>().ToList();
            _listView = _dockPanel.Children.OfType<ListView>().FirstOrDefault();
        }

        [MonitoredTest("Chapters - Should not have changed the codebehind file")]
        public void _01_ShouldNotHaveChangedTheCodebehindFile()
        {
            var codeBehindFilePath = @"Exercise2\Controls\Chapters.xaml.cs";
            string hash = Solution.Current.GetFileHash(codeBehindFilePath);
            Assert.That(hash, Is.EqualTo("F9-E8-09-19-45-A7-B9-BD-2C-19-83-66-59-BC-31-5E"),
                $"The file '{codeBehindFilePath}' has changed. " +
                "Undo your changes on the file to make this test pass.");
        }

        [MonitoredTest("Chapters - Should have all controls")]
        public void _02_ShouldHaveAllControls()
        {
            Assert.That(_dockPanel, Is.Not.Null, "The Content of the control should be a DockPanel");
            Assert.That(_textBlocks, Has.Count.EqualTo(2), "The DockPanel should have 2 children that are TextBlocks");
            Assert.That(_rectangles, Has.Count.EqualTo(2), "The DockPanel should have 2 children that are Rectangles");
            Assert.That(_listView, Is.Not.Null, "The DockPanel should have a child that is a ListView");
        }

        [MonitoredTest("Chapters - Should dock elements correctly")]
        public void _03_ShouldDockElementsCorrectly()
        {
            _02_ShouldHaveAllControls();

            TextBlock titleTextBlock = GetTitleTextBlock();
            TextBlock footerTextBlock = GetFooterTextBlock();
            Assert.That(_rectangles.Any(r => DockPanel.GetDock(r) == Dock.Left), Is.True,
                "One of the Rectangles should be docked on the left");
            Assert.That(_rectangles.Any(r => DockPanel.GetDock(r) == Dock.Right), Is.True,
                "One of the Rectangles should be docked on the right");
            Assert.That(DockPanel.GetDock(_listView), Is.EqualTo(Dock.Left), "The ListView should be docked on the left");

            int titleIndex = _dockPanel.Children.IndexOf(titleTextBlock);
            int footerIndex = _dockPanel.Children.IndexOf(footerTextBlock);
            int rectangle1Index = _dockPanel.Children.IndexOf(_rectangles.ElementAt(0));
            int rectangle2Index = _dockPanel.Children.IndexOf(_rectangles.ElementAt(1));
            int listViewIndex = _dockPanel.Children.IndexOf(_listView);

            string orderIncorrectMessage = "The order in which the elements are docked is not correct";
            Assert.That(titleIndex < rectangle1Index && titleIndex < rectangle2Index, Is.True, orderIncorrectMessage);
            Assert.That(footerIndex < rectangle1Index && footerIndex < rectangle2Index, Is.True, orderIncorrectMessage);
            Assert.That(listViewIndex > rectangle1Index && listViewIndex > rectangle2Index, Is.True, orderIncorrectMessage);
        }

        [MonitoredTest("Chapters - Should visualize each textBlock correctly")]
        public void _04_ShouldVisualizeEachElementCorrectly()
        {
            _02_ShouldHaveAllControls();

            TextBlock titleTextBlock = GetTitleTextBlock();
            Assert.That(titleTextBlock.Background, Is.InstanceOf<SolidColorBrush>(),
                "The Background of the title should be a solid color");
            Assert.That(string.IsNullOrEmpty(titleTextBlock.Text), Is.False, "The title TextBlock has no Text");
            Assert.That(titleTextBlock.TextAlignment, Is.EqualTo(TextAlignment.Center), "The Text of the title should be centered");
            Assert.That(titleTextBlock.HorizontalAlignment, Is.EqualTo(HorizontalAlignment.Stretch),
                "The title TextBlock should take all available horizontal space");
            Assert.That(titleTextBlock.FontSize, Is.GreaterThan(15), "The font size of the title should be bigger");
            Assert.That(titleTextBlock.FontWeight, Is.EqualTo(FontWeights.UltraBold), "The font weight of the title should be ultra bold");
            Assert.That(HasPadding(titleTextBlock), Is.True, "The title should have some padding on all sides");

            TextBlock footerTextBlock = GetTitleTextBlock();
            Assert.That(footerTextBlock.Background, Is.InstanceOf<SolidColorBrush>(),
                "The Background of the footer should be a solid color");
            Assert.That(string.IsNullOrEmpty(footerTextBlock.Text), Is.False, "The footer TextBlock has no Text");
            Assert.That(footerTextBlock.TextAlignment, Is.EqualTo(TextAlignment.Center), "The Text of the footer should be centered");
            Assert.That(footerTextBlock.HorizontalAlignment, Is.EqualTo(HorizontalAlignment.Stretch),
                "The footer TextBlock should take all available horizontal space");
            Assert.That(HasPadding(footerTextBlock), Is.True, "The footer should have some padding on all sides");

            Assert.That(_rectangles.All(r => r.Width > 0), Is.True, "The rectangles should have a Width set");
            Assert.That(_rectangles.All(r => r.Fill is SolidColorBrush), Is.True, "The Fill of the rectangles should be a solid color");
            Assert.That(_rectangles.All(HasMargin), Is.True, "The rectangles should have some Margin on all sides");

            Assert.That(HasMargin(_listView), Is.True, "The ListView should have some Margin on all sides");
            Assert.That(_listView.HorizontalContentAlignment, Is.EqualTo(HorizontalAlignment.Center),
                "The ListView should align its contents in the center. Tip: ContentAlignment instead of Alignment");
            List<ListViewItem> items = _listView.Items.OfType<ListViewItem>().ToList();
            Assert.That(items, Has.Count.EqualTo(8), "The ListView should contain 8 ListViewItems");
            Assert.That(items.All(item => item.Content is string content && !string.IsNullOrEmpty(content)), Is.True,
                "The content of each item should be a non-empty string");
        }

        private TextBlock GetTitleTextBlock()
        {
            TextBlock? titleTextBlock = _textBlocks.FirstOrDefault(t => DockPanel.GetDock(t) == Dock.Top);
            Assert.That(titleTextBlock, Is.Not.Null, "Cannot find a (title) TextBlock that is docked at the top");
            return titleTextBlock;
        }

        private TextBlock GetFooterTextBlock()
        {
            TextBlock? footerTextBlock = _textBlocks.FirstOrDefault(t => DockPanel.GetDock(t) == Dock.Bottom);
            Assert.That(footerTextBlock, Is.Not.Null, "Cannot find a (footer) TextBlock that is docked at the bottom");
            return footerTextBlock;
        }

        private bool HasPadding(TextBlock textBlock)
        {
            return textBlock.Padding.Left > 0 &&
                   textBlock.Padding.Top > 0 &&
                   textBlock.Padding.Right > 0 &&
                   textBlock.Padding.Bottom > 0;
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