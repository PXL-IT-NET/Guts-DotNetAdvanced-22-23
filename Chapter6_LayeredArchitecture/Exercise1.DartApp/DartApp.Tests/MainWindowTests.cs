using DartApp.Presentation;
using Moq;
using DartApp.AppLogic.Contracts;
using DartApp.Domain.Contracts;
using DartApp.Tests.Builders;
using System.Windows.Controls;
using Guts.Client.WPF.TestTools;

namespace DartApp.Tests
{
    [ExerciseTestFixture("dotnet2", "H06", "Exercise01",
    @"DartApp.Presentation\MainWindow.xaml.cs")]
    [Apartment(ApartmentState.STA)]
    public class MainWindowTests
    {
        protected static Random Random = new Random();

        private Mock<IPlayerService> _playerServiceMock = null!;
        private MainWindow _window = null!;
        private List<IPlayer> _allPlayers = null!;

        [SetUp]
        public void BeforeEachTest()
        {
            _allPlayers = new List<IPlayer>();
            for (int i = 0; i < Random.Next(2, 11); i++)
            {
                _allPlayers.Add(new PlayerBuilder().Build());
            }
            _playerServiceMock = new Mock<IPlayerService>();
            _playerServiceMock.Setup(repo => repo.GetAllPlayers()).Returns(_allPlayers);
            _window = new MainWindow(_playerServiceMock.Object);
            _window.Show();
        }

        [TearDown]
        public void AfterEachTest()
        {
            _window.Close();
        }

        [MonitoredTest("MainWindow - Constructor should retrieve all players and setup databinding")]
        public void _01_Constructor_ShouldRetrieveAllPlayersAndSetupDatabinding()
        {
            _playerServiceMock.Verify(service => service.GetAllPlayers(), Times.Once,
                "The 'GetAllPlayers' method of the player service should be used.");

            Assert.That(_window.AllPlayers, Is.EquivalentTo(_allPlayers),
                "The player instances returned by the service (and thus the repository) should end up in the 'AllPlayers' property.");

            Assert.That(_window.DataContext, Is.SameAs(_window),
                "The window should be its own DataContext.");
        }

        [MonitoredTest("MainWindow - Selected player should notify changes")]
        public void _02_SelectedPlayer_ShouldNotifyChanges()
        {
            //Arrange
            IPlayer player = new PlayerBuilder().Build();
            bool selectedPlayerChangeNotified = false;
            bool showSelectedPlayerChangeNotified = false;
            _window.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(MainWindow.SelectedPlayer))
                {
                    selectedPlayerChangeNotified = true;
                }

                if (args.PropertyName == nameof(MainWindow.ShowSelectedPlayer))
                {
                    showSelectedPlayerChangeNotified = true;
                }
            };

            //Act
            _window.SelectedPlayer = player;

            //Assert
            Assert.That(_window.SelectedPlayer, Is.SameAs(player),
                "The 'SelectedPlayer' property is not set correctly.");

            Assert.That(selectedPlayerChangeNotified, Is.True,
                "The 'PropertyChanged' event should be triggered for the property 'SelectedPlayer'.");

            Assert.That(showSelectedPlayerChangeNotified, Is.True,
                "The 'PropertyChanged' event should also be triggered for the property 'ShowSelectedPlayer' " +
                "(This property might return a different value when the selected player has changed).");
        }

        [MonitoredTest("MainWindow - A selection in the combobox should set SelectedPlayer")]
        public void _03_ComboBoxSelection_ShouldSetSelectedPlayer()
        {
            ComboBox? comboBox = _window.FindVisualChildren<ComboBox>().FirstOrDefault();
            Assert.That(comboBox, Is.Not.Null, "Could not find a ComboBox in the window.");
            Assert.That(comboBox!.Items.Count, Is.GreaterThan(0),
                "The ComboBox should contain some items. " +
                $"Maybe you need to make the test '{nameof(_01_Constructor_ShouldRetrieveAllPlayersAndSetupDatabinding)}' green first.");

            comboBox.SelectedIndex = (comboBox.SelectedIndex + 1) % _allPlayers.Count;

            //Assert
            Assert.That(_window.SelectedPlayer, Is.SameAs(comboBox.SelectedValue),
                "The 'SelectedPlayer' is not the same instance as the selected player in the ComboBox.");
        }

        [MonitoredTest("AddPlayerButton - Click should use the service and update the UI")]
        public void _04_AddPlayerButtonClick_ShouldUseTheServiceAndUpdateTheUI()
        {
            Button? addPlayerButton = _window.FindVisualChildren<Button>().FirstOrDefault(b => (b.Content as string) == "Add Player");
            Assert.That(addPlayerButton, Is.Not.Null, "Could not find a Button with content 'Add Player'.");

            TextBox? playerNameTextBox = _window.FindVisualChildren<TextBox>().FirstOrDefault(tb => tb.Name == "PlayerNameTextBox");
            Assert.That(playerNameTextBox, Is.Not.Null, "Could not find a TextBox with the name 'PlayerNameTextBox'.");

            string playerName = Guid.NewGuid().ToString();
            playerNameTextBox!.Text = playerName;

            IPlayer newPlayer = new PlayerBuilder().WithName(playerName).Build();

            IPlayer? addedPlayer = null;
            _playerServiceMock.Setup(service => service.AddPlayer(It.IsAny<string>())).Returns(newPlayer)
                .Callback((string mockPlayerAdded) =>
                {
                    addedPlayer = newPlayer;
                });

            addPlayerButton!.FireClickEvent();

            Assert.That(addedPlayer, Is.Not.Null, "The 'AddPlayer' method of the service should be called.");
            Assert.That(addedPlayer!.Name, Is.EqualTo(playerName), "The player that is passed in should contain the name filled in in the TextBox.");

            Assert.That(_window.AllPlayers, Contains.Item(addedPlayer),
                "The player that is added in the service should also be added to the 'AllPlayers' collection.");

            Assert.That(_window.SelectedPlayer, Is.SameAs(addedPlayer),
                "The player that is added in the service should also be the 'SelectedPlayer'.");

            Assert.That(playerNameTextBox.Text, Is.Empty, "After adding the player, the TextBox should be cleared.");
        }

        [MonitoredTest("AddGameResultButton - Should add a GameResult to the list and update the UI")]
        public void _05_AddGameResultButtonClick_ShouldAddAGameResultToListAndUpdateUI()
        {
            _04_AddPlayerButtonClick_ShouldUseTheServiceAndUpdateTheUI();

            Button? addGameResultButton = _window.FindVisualChildren<Button>().FirstOrDefault(b => (b.Content as string) == "Add Game Result");
            Assert.That(addGameResultButton, Is.Not.Null, "Could not find a Button with content 'Add Game Result'.");

            TextBox? gameResultAverageTextBox = _window.FindVisualChildren<TextBox>().FirstOrDefault(tb => tb.Name == "GameResultAverageTextBox");
            Assert.That(gameResultAverageTextBox, Is.Not.Null, "Could not find a TextBox with the name 'GameResultAverageTextBox'.");
            TextBox? gameResultNumberOf180TextBox = _window.FindVisualChildren<TextBox>().FirstOrDefault(tb => tb.Name == "GameResultNumberOf180TextBox");
            Assert.That(gameResultNumberOf180TextBox, Is.Not.Null, "Could not find a TextBox with the name 'GameResultNumberOf180TextBox'.");
            TextBox? gameResultBestThrowTextBox = _window.FindVisualChildren<TextBox>().FirstOrDefault(tb => tb.Name == "GameResultBestThrowTextBox");
            Assert.That(gameResultBestThrowTextBox, Is.Not.Null, "Could not find a TextBox with the name 'GameResultBestThrowTextBox'.");

            gameResultAverageTextBox!.Text = "50.0";
            gameResultNumberOf180TextBox!.Text = "2";
            gameResultBestThrowTextBox!.Text = "180";

            addGameResultButton!.FireClickEvent();

            _playerServiceMock.Verify(service => service.AddGameResultForPlayer(It.IsAny<IPlayer>(), It.IsAny<int>(), It.IsAny<double>(), It.IsAny<int>()), Times.Once,
                "The 'AddGameResult' method should be called on the service for the selected player. ");

            Assert.That(gameResultAverageTextBox.Text, Is.Empty, "After adding the Game result, the TextBox of the Average Throw should be cleared.");
            Assert.That(gameResultNumberOf180TextBox.Text, Is.Empty, "After adding the Game result, the TextBox of the number of " +
                "180s should be cleared.");
            Assert.That(gameResultBestThrowTextBox.Text, Is.Empty, "After adding the Game result, the TextBox of the Best Throw should be cleared.");
        }

        [MonitoredTest("CalculateStatisticsButton - Click should trigger the player service")]
        public void _06_CalculateStatisticsButtonClick_ShouldTriggerPLayerService()
        {
            _04_AddPlayerButtonClick_ShouldUseTheServiceAndUpdateTheUI();

            Button? calculateStatsButton = _window.FindVisualChildren<Button>().FirstOrDefault(b => (b.Content as string) == "Calculate Statistics");
            Assert.That(calculateStatsButton, Is.Not.Null, "Could not find a Button with content 'Calculate Statistics'.");

            IPlayer? selectedPlayer = _window.SelectedPlayer;
            IPlayer? playerUsedForStats = null;

            _playerServiceMock.Setup(service => service.GetStatsForPlayer(It.IsAny<IPlayer>()))
                .Callback((IPlayer mockPlayer) =>
                {
                    playerUsedForStats = mockPlayer;
                });

            calculateStatsButton!.FireClickEvent();

            _playerServiceMock.Verify(service => service.GetStatsForPlayer(It.IsAny<IPlayer>()), Times.Once,
                "The 'GetStatsForPlayer' method should be called once on the service for the selected player. ");
            Assert.That(playerUsedForStats, Is.SameAs(selectedPlayer),
                "The player that is selected in the combobox should be the same as the player the statistics are calculated for.");
        }
    }
}
