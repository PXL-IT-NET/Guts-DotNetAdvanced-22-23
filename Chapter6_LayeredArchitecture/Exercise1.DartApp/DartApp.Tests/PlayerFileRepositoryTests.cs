using DartApp.AppLogic.Contracts;
using DartApp.Domain.Contracts;
using DartApp.Infrastructure.Storage;
using DartApp.Tests.Builders;

namespace DartApp.Tests
{
    [ExerciseTestFixture("dotnet2", "H06", "Exercise01",
    @"DartApp.Infrastructure\Storage\PlayerFileRepository.cs")]
    public class PlayerFileRepositoryTests
    {
        private PlayerFileRepository _repository = null!;
        private string _playerTestDirectory = null!;

        [SetUp]
        public void BeforeEachTest()
        {
            _playerTestDirectory = Path.Combine(TestContext.CurrentContext.WorkDirectory, "testplayers");
            _repository = new PlayerFileRepository(_playerTestDirectory);
        }

        [TearDown]
        public void AfterEachTest()
        {
            if (Directory.Exists(_playerTestDirectory))
            {
                Directory.Delete(_playerTestDirectory, true);
            }
        }

        [MonitoredTest("IPlayerRepository - Should not have changed interface")]
        public void _01_ShouldNotHaveChangedIPlayerRepository()
        {
            var filePath = @"DartApp.AppLogic\Contracts\IPlayerRepository.cs";
            var fileHash = Solution.Current.GetFileHash(filePath);
            Assert.That(fileHash, Is.EqualTo("0D-A0-70-2C-87-70-4C-04-97-AF-20-16-74-F3-5E-40"),
                $"The file '{filePath}' has changed. " +
                "Undo your changes on the file to make this test pass.");
        }

        [MonitoredTest("PlayerFileRepository - Should implement IPlayerRepository")]
        public void _02_ShouldImplementIPlayerRepository()
        {
            var type = typeof(PlayerFileRepository);
            Assert.That(typeof(IPlayerRepository).IsAssignableFrom(type), Is.True);
        }

        [MonitoredTest("PlayerFileRepository - Should only be visible to the infrastructure layer")]
        public void _03_ShouldOnlyBeVisibleToTheInfrastructureLayer()
        {
            var type = typeof(PlayerFileRepository);
            Assert.That(type.IsNotPublic);
        }

        [MonitoredTest("PlayerFileRepository - Constructor - Should create the file directory")]
        public void _04_Constructor_ShouldCreateTheFileDirectory()
        {
            Assert.That(Directory.Exists(_playerTestDirectory), Is.True,
                "The constructor should create the '_playerTestDirectory' if it does not exist yet.");
        }

        [MonitoredTest("PlayerFileRepository - Add - Should save a json version of the player in a file")]
        public void _05_Add_ShouldSaveAJsonVersionOfThePlayerInAFile()
        {
            //Arrange
            IPlayer player = new PlayerBuilder().Build();

            //Act
            _repository.Add(player);

            //Assert
            AssertThatPlayerFileExists(player);
        }

        [MonitoredTest("PlayerFileRepository - GetAll - Should retrieve all added players")]
        public void _06_GetAll_ShouldRetrieveAllAddedPlayers()
        {
            //Arrange
            IPlayer playerOne = new PlayerBuilder().Build();
            IPlayer playerTwo = new PlayerBuilder().Build();

            try
            {
                _repository.Add(playerOne);
                _repository.Add(playerTwo);
            }
            catch (Exception)
            {
                Assert.Fail(
                    $"Make sure that the test '{nameof(_05_Add_ShouldSaveAJsonVersionOfThePlayerInAFile)}' is green, before attempting to make this test green.");
            }

            //Act
            IReadOnlyList<IPlayer> allPlayers = _repository.GetAll();

            //Assert
            Assert.That(allPlayers.Count, Is.EqualTo(2), "2 players should be returned after adding 2 players.");

            var playerMatch1 = allPlayers.FirstOrDefault(pl => pl.Id == playerOne.Id);
            Assert.That(playerMatch1, Is.Not.Null);
            AssertPlayerEquality(playerMatch1!, playerOne, "The first added player is not the same than the matching retrieved player.");

            var playerMatch2 = allPlayers.FirstOrDefault(p2 => p2.Id == playerTwo.Id);
            Assert.That(playerMatch2, Is.Not.Null);
            AssertPlayerEquality(playerMatch2!, playerTwo, "The second added player is not the same than the matching retrieved player.");
        }

        [MonitoredTest("PlayerFileRepository - SaveChanges - Should overwrite the matching player file")]
        public void _07_SaveChanges_ShouldOverwriteTheMatchingPlayerFile()
        {
            //Arrange
            TestPlayer? player = new PlayerBuilder().Build() as TestPlayer;
            Assert.That(player, Is.Not.Null);
            try
            {
                _repository.Add(player);
            }
            catch (Exception)
            {
                Assert.Fail(
                    $"Make sure that the test '{nameof(_05_Add_ShouldSaveAJsonVersionOfThePlayerInAFile)}' is green, before attempting to make this test green.");
            }

            string newName = "EditedName";

            //Act
            player!.Name = newName;
            _repository.SaveChanges(player);

            //Assert
            IReadOnlyList<IPlayer>? allPlayers = null;
            try
            {
                allPlayers = _repository.GetAll();
            }
            catch (Exception)
            {
                Assert.Fail(
                    $"Make sure that the test '{nameof(_06_GetAll_ShouldRetrieveAllAddedPlayers)}' is green, before attempting to make this test green.");
            }
            Assert.That(allPlayers, Is.Not.Null);
            Assert.That(allPlayers!.Count, Is.EqualTo(1), "Only one player should be returned by 'GetAll' after adding 1 player and updating it.");

            IPlayer updatedPlayer = allPlayers.First();

            Assert.That(updatedPlayer.Name, Is.EqualTo(newName), "A chage in the name is not saved correctly.");
        }

        private void AssertThatPlayerFileExists(IPlayer player)
        {
            string expectedFilePath = Path.Combine(_playerTestDirectory, $"Player_{player.Id}.json");
            Assert.That(File.Exists(expectedFilePath), Is.True,
                $"After adding a player with id {player.Id}, a file '{expectedFilePath}' should exist.");
        }

        private void AssertPlayerEquality(IPlayer player1, IPlayer player2, string errorMessage)
        {
            Assert.That(player1.Id, Is.EqualTo(player2.Id), $"{errorMessage} - The id's don't match.");
            Assert.That(player1.Name, Is.EqualTo(player2.Name), $"{errorMessage} - The names don't match.");
            Assert.That(player1.GameResults, Is.Not.Null, $"{errorMessage} - The GameResults of one of the players is null.");
            Assert.That(player2.GameResults, Is.Not.Null, $"{errorMessage} - The GameResults of one of the players is null.");
            Assert.That(player1.GameResults.Count, Is.EqualTo(player2.GameResults.Count), $"{errorMessage} - The game results don't match.");
        }
    }
}
