using System.Windows;
using System.Windows.Controls;
using Guts.Client.Core;
using Guts.Client.WPF.TestTools;

namespace Exercise1.Tests
{
    [ExerciseTestFixture("dotNet2", "H01", "Exercise01", @"Exercise1\MainWindow.xaml;Exercise1\MainWindow.xaml.cs")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        private MainWindow? _window;
        private Grid? _grid;
        private TextBlock? _titleTextBlock;
        private TextBox? _key1TextBox;
        private PasswordBox? _key2PasswordBox;
        private CheckBox? _key3CheckBox;
        private ComboBox? _key4ComboBox;
        private GroupBox? _key5GroupBox;
        private List<RadioButton> _key5RadioButtons = new();
        private Button? _submitButton;
        private Border? _feedbackBorder;
        private TextBlock? _feedbackTextBlock;

        [SetUp]
        public void BeforeEachTest()
        {
            _window = new MainWindow();
            _window.Show();

            _grid = _window.Content as Grid;
            if (_grid == null) return;

            _titleTextBlock = _grid.Children.OfType<TextBlock>().FirstOrDefault();

            _key1TextBox = _grid.FindVisualChildren<TextBox>().FirstOrDefault();
            _key2PasswordBox = _grid.FindVisualChildren<PasswordBox>().FirstOrDefault();
            _key3CheckBox = _grid.FindVisualChildren<CheckBox>().FirstOrDefault();
            _key4ComboBox = _grid.FindVisualChildren<ComboBox>().FirstOrDefault();
            _key5GroupBox = _grid.FindVisualChildren<GroupBox>().FirstOrDefault();
            _key5RadioButtons = new List<RadioButton>();
            if (_key5GroupBox != null)
            {
                _key5RadioButtons = _key5GroupBox.FindVisualChildren<RadioButton>().ToList();
            }

            _submitButton = _grid.FindVisualChildren<Button>().FirstOrDefault();

            _feedbackBorder = _grid.Children.OfType<Border>().FirstOrDefault();
            if (_feedbackBorder != null)
            {
                _feedbackTextBlock = _feedbackBorder.FindVisualChildren<TextBlock>().FirstOrDefault();
            }
        }

        [TearDown]
        public void AfterEachTest()
        {
            _window?.Close();
        }

        [MonitoredTest("Should have all controls")]
        public void _01_ShouldHaveAllControls()
        {
            Assert.That(_grid, Is.Not.Null, "The Content of the Window should be a Grid");
            Assert.That(_titleTextBlock, Is.Not.Null, "No TextBlock control is found for the title");
            Assert.That(_key1TextBox, Is.Not.Null, "No TextBox control is found for key1");
            Assert.That(_key2PasswordBox, Is.Not.Null, "No PasswordBox control is found for key2");
            Assert.That(_key3CheckBox, Is.Not.Null, "No CheckBox control is found for key3");
            Assert.That(_key4ComboBox, Is.Not.Null, "No ComboBox control is found for key4");
            List<ComboBoxItem> comboBoxItems = _key4ComboBox.Items.OfType<ComboBoxItem>().ToList();
            Assert.That(comboBoxItems, Has.Count.EqualTo(3), "The ComboBox for key4 should have 3 ComboBoxItems");
            Assert.That(_key5GroupBox, Is.Not.Null, "No GroupBox control is found for key5. The RadioButtons should be inside a GroupBox");
            Assert.That(_key5RadioButtons, Has.Count.EqualTo(3),
                "There should be 3 RadioButtons in the GroupBox. " +
                "Tip: a GroupBox can only contain 1 content element, but there are layout elements that can contain multiple elements....");
            Assert.That(_key5RadioButtons.First().IsChecked, Is.True, "The first RadioButton of key5 should be checked by default");
            Assert.That(_submitButton, Is.Not.Null, "No Button control is found");
            Assert.That(_feedbackBorder, Is.Not.Null, "No Border control that can contain the feedback TextBlock is found");
            Assert.That(_feedbackTextBlock, Is.Not.Null, "No TextBlock control that can contain the feedback is found inside the feedback Border");
        }

        [MonitoredTest("Should have its controls arranged in a Grid")]
        public void _02_ShouldHaveItsControlsArrangedInAGrid()
        {
            _01_ShouldHaveAllControls();

            Assert.That(_grid.ShowGridLines, Is.True, "The Grid should show its grid lines");
            Assert.That(_grid.RowDefinitions.Count, Is.EqualTo(4), "The Grid should have 4 rows");
            Assert.That(_grid.ColumnDefinitions.Count, Is.EqualTo(3), "The Grid should have 3 columns");

            Assert.That(_grid.ColumnDefinitions,
                Has.All.Matches((ColumnDefinition cd) => cd.Width == new GridLength(1, GridUnitType.Star)),
                "All Grid columns should take an equal amount of the available space.");

            RowDefinition row1Definition = _grid.RowDefinitions[0];
            Assert.That(row1Definition.Height, Is.EqualTo(GridLength.Auto),
                "The first row of the Grid should be as high as needed to display its children (title)");

            RowDefinition row2Definition = _grid.RowDefinitions[1];
            Assert.That(row2Definition.Height, Is.EqualTo(new GridLength(2, GridUnitType.Star)),
                "The second row of the Grid should take up 2/7th of the available space");

            RowDefinition row3Definition = _grid.RowDefinitions[2];
            Assert.That(row3Definition.Height, Is.EqualTo(new GridLength(5, GridUnitType.Star)),
                "The third row of the Grid should take up 5/7th of the available space");

            RowDefinition row4Definition = _grid.RowDefinitions[3];
            Assert.That(row4Definition.Height, Is.EqualTo(GridLength.Auto),
                "The last row of the Grid should be as high as needed to display its children (feedback Border)");

            AssertGridPosition(_titleTextBlock, 0, 0, 1, 3);
            AssertGridPosition(_key1TextBox, 1, 0);
            AssertGridPosition(_key2PasswordBox, 1, 1);
            AssertGridPosition(_key3CheckBox, 1, 2);
            AssertGridPosition(_key4ComboBox, 2, 0);
            AssertGridPosition(_key5GroupBox, 2, 1);
            AssertGridPosition(_submitButton, 2, 2);
            AssertGridPosition(_feedbackBorder, 3, 0, 1, 3);
        }

        [MonitoredTest("Should have the title configured correctly")]
        public void _03_ShouldHaveTitleConfiguredCorrectly()
        {
            _02_ShouldHaveItsControlsArrangedInAGrid();

            Assert.That(string.IsNullOrEmpty(_titleTextBlock.Text), Is.False, "Should have a text set");
            Assert.That(_titleTextBlock.HorizontalAlignment, Is.EqualTo(HorizontalAlignment.Center), "Should be in the center");
            Assert.That(_titleTextBlock.FontSize, Is.GreaterThan(16.0), "Font size should be bigger");
            Assert.That(_titleTextBlock.FontWeight, Is.EqualTo(FontWeights.Bold), "Should be Bold");
            Assert.That(HasMargin(_titleTextBlock), Is.True, "Should have some Margin on all sides");
        }

        [MonitoredTest("Should have the code keys configured correctly")]
        public void _04_ShouldHaveCodeKeysConfiguredCorrectly()
        {
            _02_ShouldHaveItsControlsArrangedInAGrid();

            Assert.That(_key1TextBox.Width > 0 && _key1TextBox.Height > 0, Is.True,
                "Key1 TextBox should have a fixed width and height");
            Assert.That(_key2PasswordBox.Width > 0 && _key2PasswordBox.Height > 0, Is.True,
                "Key2 PasswordBox should have a fixed width and height");
            Assert.That(_key3CheckBox.Width > 0 && _key3CheckBox.Height > 0, Is.True,
                "Key3 CheckBox should have a fixed width and height");
            Assert.That(double.IsNaN(_key4ComboBox.Width) && double.IsNaN(_key4ComboBox.Height), Is.True,
                "Key4 ComboBox should NOT have a fixed width or fixed height");
            Assert.That(
                _key4ComboBox.HorizontalAlignment == HorizontalAlignment.Center &&
                _key4ComboBox.VerticalAlignment == VerticalAlignment.Center, Is.True,
                "Key4 ComboBox should be in the center of the available space");
            Assert.That(double.IsNaN(_key5GroupBox.Width) && double.IsNaN(_key5GroupBox.Height), Is.True,
                "Key5 GroupBox should NOT have a fixed width or fixed height");
            Assert.That(
                _key5GroupBox.HorizontalAlignment == HorizontalAlignment.Center &&
                _key5GroupBox.VerticalAlignment == VerticalAlignment.Center, Is.True,
                "Key5 GroupBox should be in the center of the available space");
            Assert.That(_key5GroupBox.Header is string header && !string.IsNullOrEmpty(header), Is.True,
                "Key5 GroupBox should have a non-empty string in its header");
        }

        [MonitoredTest("Should have the submit Button configured correctly")]
        public void _05_ShouldHaveSubmitButtonConfiguredCorrectly()
        {
            _02_ShouldHaveItsControlsArrangedInAGrid();

            Assert.That(double.IsNaN(_submitButton.Width) && double.IsNaN(_submitButton.Height), Is.True,
                "Should NOT have a fixed width or fixed height");
            Assert.That(
                _submitButton.HorizontalAlignment == HorizontalAlignment.Stretch &&
                _submitButton.VerticalAlignment == VerticalAlignment.Stretch, Is.True,
                "Should fill the available space");
            Assert.That(HasMargin(_submitButton), Is.True, "Should have some Margin on all sides");
            StackPanel? stackPanel = _submitButton.Content as StackPanel;
            Assert.That(stackPanel is { Orientation: Orientation.Vertical }, Is.True,
                "Should display 2 TextBlocks underneath each other");
            List<TextBlock> buttonTextBlocks = stackPanel.Children.OfType<TextBlock>().ToList();
            Assert.That(buttonTextBlocks, Has.Count.EqualTo(2), "Should display 2 TextBlocks underneath each other");
            TextBlock infoTextBlock = buttonTextBlocks[0];
            TextBlock actionTextBlock = buttonTextBlocks[1];
            Assert.That(infoTextBlock.Text, Is.Not.Empty, "The first text line of the button should contain some text");
            Assert.That(actionTextBlock.Text, Is.Not.Empty, "The second text line of the button should contain some text");
            Assert.That(actionTextBlock.HorizontalAlignment, Is.EqualTo(HorizontalAlignment.Center),
                "The second text line of the button should be in the center");
            Assert.That(actionTextBlock.FontSize, Is.GreaterThan(15.0),
                "Font size of the second text line of the button should be bigger");
            Assert.That(actionTextBlock.FontWeight, Is.EqualTo(FontWeights.Bold),
                "The second text line of the button should be Bold");
        }

        [MonitoredTest("Should have the feedback Border configured correctly")]
        public void _06_ShouldHaveFeedbackBorderConfiguredCorrectly()
        {
            _02_ShouldHaveItsControlsArrangedInAGrid();

            Assert.That(double.IsNaN(_feedbackBorder.Width) && double.IsNaN(_feedbackBorder.Height), Is.True,
                "Should NOT have a fixed width or fixed height");
            Assert.That(
                _feedbackBorder.HorizontalAlignment == HorizontalAlignment.Stretch &&
                _feedbackBorder.VerticalAlignment == VerticalAlignment.Stretch, Is.True,
                "Should fill the available space");
            Assert.That(HasMargin(_feedbackBorder), Is.True, "Should have some Margin on all sides");
            Assert.That(_feedbackBorder.Background, Is.Not.Null, "Should have a Background set");
            Assert.That(HasCornerRadius(_feedbackBorder), Is.True, "Should have rounded corners");
            Assert.That(_feedbackBorder.Visibility, Is.EqualTo(Visibility.Collapsed),
                "Should not be visible and should not take up any space");

            Assert.That(string.IsNullOrEmpty(_feedbackTextBlock.Text), Is.False, "Text in the Border should not be empty");
            Assert.That(_feedbackTextBlock.HorizontalAlignment, Is.EqualTo(HorizontalAlignment.Center), "Text in the Border should be in the center");
            Assert.That(_feedbackTextBlock.FontSize, Is.GreaterThan(16.0), "Text in the Border should have a bigger font size");
        }

        [MonitoredTest("SubmitButtonClick - Correct keys are filled in - Should display positive feedback")]
        public void _07_SubmitButtonClick_CorrectKeysFilledIn_ShouldDisplayPositiveFeedback()
        {
            try
            {
                _04_ShouldHaveCodeKeysConfiguredCorrectly();
                _05_ShouldHaveSubmitButtonConfiguredCorrectly();
                _06_ShouldHaveFeedbackBorderConfiguredCorrectly();
            }
            catch
            {
                Assert.Fail("Make sure previous tests are green first");
            }

            _key1TextBox.Text = "PXL";
            _key2PasswordBox.Password = "ForLife";
            _key3CheckBox.IsChecked = true;
            _key4ComboBox.SelectedIndex = 2;
            _key5RadioButtons.ElementAt(1).IsChecked = true;

            _submitButton.FireClickEvent();

            Assert.That(_feedbackBorder.Visibility, Is.EqualTo(Visibility.Visible),
                "The feedback Border should be visible");
            Assert.That(_feedbackTextBlock.Text, Does.Contain("you cracked the code").IgnoreCase);

        }

        [MonitoredTest("SubmitButtonClick - 3 attempts with invalid keys filled in - Should display negative feedback and show the remaining attempts")]
        public void _08_SubmitButtonClick_3AttemptsWithInvalidKeysFilledIn_ShouldDisplayNegativeFeedbackAndShowRemainingAttempts()
        {
            try
            {
                _04_ShouldHaveCodeKeysConfiguredCorrectly();
                _05_ShouldHaveSubmitButtonConfiguredCorrectly();
                _06_ShouldHaveFeedbackBorderConfiguredCorrectly();
            }
            catch
            {
                Assert.Fail("Make sure previous tests are green first");
            }

            int expectedAttemptsLeft = 4;
            AssertInvalidAttempt("Thomas More", "Forever", false, 1, 2, expectedAttemptsLeft);
            expectedAttemptsLeft--;
            AssertInvalidAttempt("PXL", "ForLife", true, 2, 2, expectedAttemptsLeft);
            expectedAttemptsLeft--;
            AssertInvalidAttempt("PXL", "ForLife", false, 2, 1, expectedAttemptsLeft);
        }

        [MonitoredTest("SubmitButtonClick - 5 invalid attempts - Should display negative feedback and disable the submit Button")]
        public void _09_SubmitButtonClick_5InvalidAttempts_ShouldDisplayNegativeFeedbackAndDisableSubmitButton()
        {
            try
            {
                _04_ShouldHaveCodeKeysConfiguredCorrectly();
                _05_ShouldHaveSubmitButtonConfiguredCorrectly();
                _06_ShouldHaveFeedbackBorderConfiguredCorrectly();
            }
            catch
            {
                Assert.Fail("Make sure previous tests are green first");
            }

            for (int i = 0; i < 5; i++)
            {
                _key1TextBox.Text = "Wrong";
                _key2PasswordBox.Password = "Worse";
                _key3CheckBox.IsChecked = false;
                _key4ComboBox.SelectedIndex = 0;
                _key5RadioButtons.ElementAt(0).IsChecked = true;
                _submitButton.FireClickEvent();
            }

            Assert.That(_feedbackBorder.Visibility, Is.EqualTo(Visibility.Visible),
                "The feedback Border should be visible");
            Assert.That(_feedbackTextBlock.Text, Does.Contain("game over").IgnoreCase);
            Assert.That(_submitButton.IsEnabled, Is.False, "The button should be disabled after 5 attempts");

        }

        private void AssertGridPosition(FrameworkElement element, int expectedRow, int expectedColumn,
            int expectedRowSpan = 1, int expectedColumnSpan = 1)
        {
            int row = Grid.GetRow(element);
            int column = Grid.GetColumn(element);
            int rowSpan = Grid.GetRowSpan(element);
            int columnSpan = Grid.GetColumnSpan(element);
            Assert.That(row, Is.EqualTo(expectedRow), $"The row of the {element.GetType().Name} should be {expectedRow}");
            Assert.That(column, Is.EqualTo(expectedColumn), $"The column of the {element.GetType().Name} should be {expectedColumn}");
            Assert.That(rowSpan, Is.EqualTo(expectedRowSpan), $"The row span of the {element.GetType().Name} should be {expectedRowSpan}");
            Assert.That(columnSpan, Is.EqualTo(expectedColumnSpan), $"The column span of the {element.GetType().Name} should be {expectedColumnSpan}");
        }
        private bool HasMargin(FrameworkElement element)
        {
            return element.Margin.Left > 0 &&
                   element.Margin.Top > 0 &&
                   element.Margin.Right > 0 &&
                   element.Margin.Bottom > 0;
        }

        private bool HasCornerRadius(Border border)
        {
            return border.CornerRadius.TopLeft > 0 &&
                   border.CornerRadius.BottomLeft > 0 &&
                   border.CornerRadius.BottomRight > 0 &&
                   border.CornerRadius.TopRight > 0;
        }

        private void AssertInvalidAttempt(string key1, string key2, bool key3IsChecked, int key4SelectedIndex,
            int key5CheckedIndex, int expectedAttemptsLeft)
        {
            _key1TextBox.Text = key1;
            _key2PasswordBox.Password = key2;
            _key3CheckBox.IsChecked = key3IsChecked;
            _key4ComboBox.SelectedIndex = key4SelectedIndex;
            _key5RadioButtons.ElementAt(key5CheckedIndex).IsChecked = true;

            _submitButton.FireClickEvent();

            Assert.That(_feedbackBorder.Visibility, Is.EqualTo(Visibility.Visible),
                "The feedback Border should be visible");
            Assert.That(_feedbackTextBlock.Text, Does.Contain("invalid code").IgnoreCase);
            Assert.That(_feedbackTextBlock.Text, Does.Contain($"{expectedAttemptsLeft} attempts left").IgnoreCase);
            Assert.That(_feedbackTextBlock.Text, Does.Not.Contain("game over").IgnoreCase);
            Assert.That(_submitButton.IsEnabled, Is.True, $"The button should not be disabled when there are {expectedAttemptsLeft} attempts left");
        }

    }
}