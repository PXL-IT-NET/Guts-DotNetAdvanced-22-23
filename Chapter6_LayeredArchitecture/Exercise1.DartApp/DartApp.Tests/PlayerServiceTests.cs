using DartApp.AppLogic;
using DartApp.AppLogic.Contracts;
using DartApp.Domain;
using DartApp.Domain.Contracts;
using DartApp.Tests.Builders;
using Moq;
using System.Reflection;

namespace DartApp.Tests
{
    [ExerciseTestFixture("dotnet2", "H06", "Exercise01",
    @"DartApp.AppLogic\PlayerService.cs")]    
    public class PlayerServiceTests
    {
        private Type _playerServiceType = null!;
        private Mock<IPlayerRepository> _playerRepositoryMock = null!;
        private PlayerService? _service = null!;

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            _playerServiceType = typeof(PlayerService);
            _playerRepositoryMock = new Mock<IPlayerRepository>();
            ConstructPlayerService(_playerRepositoryMock.Object);
        }

        [MonitoredTest("IPlayerService - Should not have changed interface")]
        public void _01_ShouldNotHaveChangedIBattleService()
        {
            var filePath = @"DartApp.AppLogic\Contracts\IPlayerService.cs";
            var fileHash = Solution.Current.GetFileHash(filePath);
            Assert.That(fileHash, Is.EqualTo("2B-A8-11-89-3D-64-DE-30-FD-B7-DC-93-12-A6-FD-01"),
                $"The file '{filePath}' has changed. " +
                "Undo your changes on the file to make this test pass.");
        }

        [MonitoredTest("PlayerService - Should implement IPlayerService")]
        public void _02_ShouldImplementIPlayerService()
        {
            Assert.That(typeof(IPlayerService).IsAssignableFrom(_playerServiceType), Is.True);
        }

        [MonitoredTest("PlayerService - Should only be visible to the application logic layer")]
        public void _03_ShouldOnlyBeVisibleToTheAppLogicLayer()
        {
            Assert.That(_playerServiceType.IsNotPublic,
                "Only IPlayerService should be visible to the other layers. The IPlayerService class itself can be encapsulated in the application logic layer.");
        }

        [MonitoredTest("PlayerService - Should have a constructor that accepts a player repository (IPlayerRepository)")]
        public void _04_ShouldHaveAConstructorThatAcceptsAPlayerRepository()
        {
            ConstructorInfo[] constructors = _playerServiceType.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
            Assert.That(constructors.Length, Is.EqualTo(1), "There should be exactly one public constructor.");

            ConstructorInfo constructor = constructors.First();
            ParameterInfo[] parameters = constructor.GetParameters();
            Assert.That(parameters.Length, Is.EqualTo(1), "The constructor should have 1 parameters.");
            Assert.That(parameters.Any(p => p.ParameterType == typeof(IPlayerRepository)), Is.True,
                "The constructor parameter should be of type IPlayerRepository.");
        }

        [MonitoredTest("PlayerService - Adding a player should invoke the repository")]
        public void _05_AddPlayerShouldInvokeRepositoryAndReturnAPlayer()
        {
            string playerName = Guid.NewGuid().ToString();

            _playerRepositoryMock.Setup(repo => repo.Add(It.IsAny<IPlayer>()));
            _playerRepositoryMock.Invocations.Clear();

            IPlayer? addedPlayer = _service?.AddPlayer(playerName);

            _playerRepositoryMock.Verify(repo => repo.Add(It.IsAny<IPlayer>()), Times.Once,
                            "The 'Add' method of the player repository should be used once.");
            Assert.That(addedPlayer, Is.Not.Null, "The service should create a Player and return it.");
            Assert.That(addedPlayer.Name, Is.EqualTo(playerName), "The service should create a Player and return it.");
        }

        [MonitoredTest("PlayerService - Get all the players should invoke the repository and return them")]
        public void _06_GetAllPlayerShouldInvokeRepositoryAndReturnPlayers()
        {
            PlayerBuilder playerBuilder = new PlayerBuilder();
            List<IPlayer> playerList = new List<IPlayer>();
            IPlayer playerOne = playerBuilder.Build();
            IPlayer playerTwo = playerBuilder.Build();
            playerList.Add(playerOne);
            playerList.Add(playerTwo);

            IReadOnlyList<IPlayer>? returnedList = null;

            _playerRepositoryMock.Setup(repo => repo.GetAll()).Returns(playerList);
            _playerRepositoryMock.Invocations.Clear();

            returnedList = _service!.GetAllPlayers();
                     
            _playerRepositoryMock.Verify(repo => repo.GetAll(), Times.Once,
                            "The 'GetAll' method of the player repository should be used once.");
            Assert.That(returnedList, Is.Not.Null, "The service should return a list of players.");
            Assert.That(returnedList.Count, Is.EqualTo(2), "The list should contain two items.");
            Assert.That(returnedList[0], Is.EqualTo(playerOne));
            Assert.That(returnedList[1], Is.EqualTo(playerTwo));
        }

        [MonitoredTest("PlayerService - Adding a game result should create a GameResult and add it to the player")]
        public void _07_AddGameResultForPlayer_ShouldCreateAndAddGameResultForPlayer()
        {
            IPlayer playerOne = ConstructEmptyPlayer();
            _playerRepositoryMock.Setup(repo => repo.SaveChanges(It.IsAny<IPlayer>()));
            _playerRepositoryMock.Invocations.Clear();

            _service?.AddGameResultForPlayer(playerOne, 0, 0, 0);

            Assert.That(playerOne.GameResults, Is.Not.Null);
            Assert.That(playerOne.GameResults.Count, Is.EqualTo(1));
        }
        [MonitoredTest("PlayerService - Adding a game result should invoke SaveChanges")]
        public void _08_AddGameResultForPlayer_ShouldSaveChangesForPlayer()
        {
            IPlayer playerOne = ConstructEmptyPlayer();
            _playerRepositoryMock.Setup(repo => repo.SaveChanges(It.IsAny<IPlayer>()));
            _playerRepositoryMock.Invocations.Clear();
            
            _service?.AddGameResultForPlayer(playerOne, 0, 0, 0);

            _playerRepositoryMock.Verify(repo => repo.SaveChanges(It.IsAny<IPlayer>()), Times.Once,
                "The 'SaveChanges' method of the player repository should be used once.");
        }

        private void ConstructPlayerService(IPlayerRepository playerRepository)
        {
            try
            {
                try
                {
                    _service = Activator.CreateInstance(typeof(PlayerService),
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null,
                        new object[] { playerRepository },
                        null) as PlayerService;
                }
                catch (Exception)
                {
                    _service = Activator.CreateInstance(typeof(PlayerService),
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null,
                        new object[] { },
                        null) as PlayerService;
                }
            } catch (Exception)
            {
                _service = null;
            }
            Assert.That(_service, Is.Not.Null, "Failed to instantiate a PlayerService.");
        }
        private IPlayer ConstructEmptyPlayer()
        {
            IPlayer? playerToContruct = null;
            try
            {
                playerToContruct = Activator.CreateInstance(typeof(Player),
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null,
                    new object[] { },
                    null) as Player;
            }
            catch (Exception)
            {
                playerToContruct = null;
            }
            Assert.That(playerToContruct, Is.Not.Null, "Failed to instantiate a Player.");
            return playerToContruct;
        }
    }
}
