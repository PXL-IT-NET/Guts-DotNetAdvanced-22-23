using System.Windows.Controls;
using Guts.Client.Core;
using Moq;
using Guts.Client.Core.TestTools;
using Guts.Client.WPF.TestTools;

namespace Exercise2.Tests;

[ExerciseTestFixture("dotNet2", "H02", "Exercise02", @"Exercise2\MainWindow.xaml;Exercise2\MainWindow.xaml.cs")]
[Apartment(ApartmentState.STA)]
public class MainWindowTests
{
    private MainWindow? _window;
    private TextBlock _blackBoardTextBlock;
    private Button _addStudentButton;
    private TextBox _firstNameTextBox;
    private TextBox _lastNameTextBox;
    private TextBox _departmentTextBox;

    [TearDown]
    public void AfterEachTest()
    {
        _window?.Close();
    }

    [MonitoredTest("MainWindow - Constructor - BlackBoard should subscribe to StudentAdministration events")]
    public void _01_Constructor_BlackBoard_ShouldSubscribeToStudentAdministrationEvents()
    {
        var administrationMock = new Mock<IStudentAdministration>();
        var blackBoardMock = new Mock<IBlackBoard>();
        InitializeWindow(administrationMock.Object, blackBoardMock.Object);

        blackBoardMock.Verify(
            bb => bb.SubscribeToStudentAdministrationEvents(administrationMock.Object, _blackBoardTextBlock),
            Times.Once, "The 'SubscribeToStudentAdministrationEvents' method is not called correctly");
    }

    [MonitoredTest("MainWindow - AddStudentButtonClick - Should register student and show output in the BlackBoard TextBlock")]
    public void _02_AddStudentButtonClick_ShouldRegisterStudentAndShowOutputInTheBlackBoardTextBlock()
    {
        var administration = new StudentAdministration();
        var blackBoard = new BlackBoard();
        InitializeWindow(administration, blackBoard);

        string firstName = Guid.NewGuid().ToString();
        string lastName = Guid.NewGuid().ToString();
        string department = Guid.NewGuid().ToString();

        _firstNameTextBox.Text = firstName;
        _lastNameTextBox.Text = lastName;
        _departmentTextBox.Text = department;

        _addStudentButton.FireClickEvent();

        Assert.That(administration.StudentTotal, Is.EqualTo(1), "Student does not seem to be registered because the StudentTotal has not risen");

        Assert.That(_blackBoardTextBlock.Text.Contains(firstName), Is.True, "The first name of the registered student should appear in the BlackBoard TextBlock");
        Assert.That(_blackBoardTextBlock.Text.Contains(lastName), Is.True, "The last name of the registered student should appear in the BlackBoard TextBlock");
        Assert.That(_blackBoardTextBlock.Text.Contains(department), Is.True, "The department of the registered student should appear in the BlackBoard TextBlock");
    }

    [MonitoredTest("MainWindow - Should not manipulate the BlackBoard TextBlock text directly")]
    public void _03_ShouldNotManipulateTheBlackBoardTextBlockTextDirectly()
    {
        string mainWindowContent = Solution.Current.GetFileContent(@"Exercise2\MainWindow.xaml.cs");
        Assert.That(mainWindowContent, Does.Not.Contain("blackBoardTextBlock.Text"));
    }

    private void InitializeWindow(IStudentAdministration administration, IBlackBoard blackBoard)
    {
        _window = new MainWindow(administration, blackBoard);
        _window.Show();

        _blackBoardTextBlock = _window.GetPrivateFieldValueByName<TextBlock>("blackBoardTextBlock");
        _addStudentButton = _window.GetPrivateFieldValueByName<Button>("addStudentButton");
        _firstNameTextBox = _window.GetPrivateFieldValueByName<TextBox>("firstNameTextBox");
        _lastNameTextBox = _window.GetPrivateFieldValueByName<TextBox>("lastNameTextBox");
        _departmentTextBox = _window.GetPrivateFieldValueByName<TextBox>("departmentTextBox");
    }

}