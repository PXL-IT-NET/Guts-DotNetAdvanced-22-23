using DartApp.Domain;
using DartApp.Domain.Contracts;
using DartApp.Tests.Builders;
using System.Reflection;

namespace DartApp.Tests
{
    [ExerciseTestFixture("dotnet2", "H06", "Exercise01",
    @"DartApp.Domain\Player.cs")]
    public class PlayerTests : TestBase
    {
        private Type _playerType = null!;

        [SetUp]
        public void Setup()
        {
            _playerType = typeof(Player);
        }

        [MonitoredTest("Player - Should implement IPlayer")]
        public void _01_ShouldImplementIPlayer()
        {
            Assert.That(typeof(IPlayer).IsAssignableFrom(_playerType), Is.True);
        }

        [MonitoredTest("IPlayer - Interface should not be changed")]
        public void _02_ShouldNotHaveChangedIPlayer()
        {
            var filePath = @"DartApp.Domain\Contracts\IPlayer.cs";
            var fileHash = Solution.Current.GetFileHash(filePath);
            Assert.That(fileHash, Is.EqualTo("F0-A1-C9-B2-FD-E9-7B-E0-56-02-81-E8-B8-2D-E0-4F"),
                $"The file '{filePath}' has changed. " +
                "Undo your changes on the file to make this test pass.");
        }

        [MonitoredTest("Player - Should only be visible to the domain layer")]
        public void _03_ShouldOnlyBeVisibleToTheDomainLayer()
        {
            Assert.That(_playerType.IsNotPublic,
                "Only IPlayer should be visible to the other layers. The Player class itself can be encapsulated in the domain layer.");
        }

        [MonitoredTest("Player - Should have a private parameter-less constructor and private setters (for json conversion to work)")]
        public void _04_ShouldHaveAPrivateParameterLessConstructorAndPrivateSettersForJsonConversionToWork()
        {
            var constructor = _playerType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(c => c.IsPrivate);

            Assert.That(constructor, Is.Not.Null, "Cannot find a private constructor.");
            Assert.That(constructor.GetParameters().Length, Is.Zero, "The private constructor should not have parameters.");

            AssertHasPrivateSetter(_playerType, nameof(IPlayer.Id));
            AssertHasPrivateSetter(_playerType, nameof(IPlayer.Name));

            AssertHasNoSetter(_playerType, nameof(IPlayer.GameResults));
        }

        [MonitoredTest("Player - Should have a constructor that accepts a name")]
        public void _05_ShouldHaveAConstructorThatAcceptsAName()
        {
            string name = Guid.NewGuid().ToString();
            CreatePlayer(name);
        }

        [MonitoredTest("Player - Constructor - Should initialize properly")]
        public void _06_Constructor_ShouldInitializeProperly()
        {
            string name = Guid.NewGuid().ToString();

            IPlayer? player = CreatePlayer(name);

            Assert.That(player, Is.Not.Null);
            Assert.That(player.Name, Is.EqualTo(name), "The 'Name' is not initialized correctly.");
            Assert.That(player.Id, Is.Not.EqualTo(Guid.Empty), "The constructor should generate and assign a Guid to the 'Id' property.");
            Assert.That(player.GameResults, Is.Not.Null, "The 'GameResults' list should be an empty list.");
            Assert.That(player.GameResults.Count, Is.Zero, "The 'GameResults' list should be an empty list.");
        }

        [MonitoredTest("Player - Constructor - Empty name - Should throw an ArgumentException")]
        public void _07_Constructor_EmptyName_ShouldThrowArgumentException()
        {
            string? invalidName = null;

            Assert.That(() => CreatePlayer(invalidName), Throws.ArgumentException,
                "An 'ArgumentException' should be thrown when the name is 'null'.");

            invalidName = "";
            Assert.That(() => CreatePlayer(invalidName), Throws.ArgumentException,
                "An 'ArgumentException' should be thrown when the name is an empty string.");
        }

        [MonitoredTest("Player - ToString - Should return a string containing the name")]
        public void _08_ToString_ShouldReturnAStringContainingName()
        {
            string name = Guid.NewGuid().ToString();

            IPlayer player = CreatePlayer(name);

            string? playerLoadAsText = player.ToString();

            Assert.That(playerLoadAsText, Is.Not.Null);
            Assert.That(playerLoadAsText, Is.EqualTo(name), "The ToString should return the Name.");
        }

        [MonitoredTest("Player - AddGameResult - Should add a GameResult")]
        public void _09_AddGameResult_ShouldAddTheGameResult()
        {
            //Arrange
            string name = Guid.NewGuid().ToString();
            IPlayer player = CreatePlayer(name);

            IGameResult gameResult = new GameResultBuilder().WithPlayerId(player.Id).Build();

            var originalGameResultCollection = player.GameResults;
            //Act
            player.AddGameResult(gameResult);

            //Assert
            Assert.That(player.GameResults.Count, Is.EqualTo(1), "The 'GameResults' property should contain 1 game result.");
            IGameResult addedResult = player.GameResults.First();
            Assert.That(addedResult, Is.Not.Null, "The added game result should not be null.");
            Assert.That(addedResult, Is.EqualTo(gameResult), "The description of the added game result is not correct.");
            Assert.That(addedResult.PlayerId, Is.EqualTo(player.Id), "The 'PlayerId' of the added GameResult is not correct.");

            Assert.That(player.GameResults, Is.SameAs(originalGameResultCollection),
                "The collection that is returned by the 'GameResults' property should be the same object in memory than the collection that is returned by the 'GameResults' property after the add of the player. " +
                "Tip1: The 'GameResults' property should not have a setter. Use a backing field. " +
                "Tip2: List<IGameResult> implements the IReadOnlyCollection<IGameResult> interface.");
        }

        [MonitoredTest("Player - GetPlayerStats - Should calculate stats correctly")]
        public void _10_GetPlayerStats_ShouldCalculateStatsCorrectly()
        {
            //Arrange
            string name = Guid.NewGuid().ToString();
            IPlayer player = CreatePlayer(name);

            IPlayerStats worstPossible = player.GetPlayerStats();
            Assert.That(worstPossible.AverageThrow, Is.EqualTo(0), "All values when GameResults is empty should be 0.");
            Assert.That(worstPossible.AverageBestThrow, Is.EqualTo(0), "All values when GameResults is empty should be 0.");
            Assert.That(worstPossible.BestThrow, Is.EqualTo(0), "All values when GameResults is empty should be 0.");
            Assert.That(worstPossible.Total180, Is.EqualTo(0), "All values when GameResults is empty should be 0.");

            double checkAverage = 0;
            double checkBestThrowAverage = 0;
            double averageThrow;
            int checkBestThrow = 0;

            for (int i = 0; i < 3; i++)
            {
                averageThrow = Random.NextDouble() * 40.0 + 20.0;
                checkAverage += averageThrow;
                int bestThrow = (int)averageThrow + (int)(Random.Next(1,10) * 10);
                if (bestThrow > checkBestThrow)
                {
                    checkBestThrow = bestThrow;
                }
                checkBestThrowAverage += bestThrow;
                TestGameResult? testGameResultMediocre = new GameResultBuilder().WithPlayerId(player.Id).Build() as TestGameResult;
                testGameResultMediocre!.NumberOf180 = 0;
                testGameResultMediocre!.AverageThrow = averageThrow;
                testGameResultMediocre!.BestThrow = bestThrow;
                player.AddGameResult(testGameResultMediocre);
            }

            IPlayerStats mediocre = player.GetPlayerStats();
            Assert.That(mediocre.AverageThrow, Is.EqualTo(checkAverage/3.0), "Average not calculated correctly.");
            Assert.That(mediocre.AverageBestThrow, Is.EqualTo(checkBestThrowAverage/3.0), "Best throw average not calculated correctly.");
            Assert.That(mediocre.Total180, Is.EqualTo(0), "No 180 thrown in the mediocre case.");
            Assert.That(mediocre.BestThrow, Is.Not.EqualTo(180), "No 180 thrown in the mediocre case.");
            Assert.That(mediocre.BestThrow, Is.EqualTo(checkBestThrow), "Best throw not calculated correctly.");

            // Add a good game
            averageThrow = Random.NextDouble() * 40.0 + 30.0;
            TestGameResult? testGameResultGood = new GameResultBuilder().WithPlayerId(player.Id).Build() as TestGameResult;
            testGameResultGood!.NumberOf180 = 3;
            testGameResultGood!.AverageThrow = averageThrow;
            testGameResultGood!.BestThrow = 180;

            player.AddGameResult(testGameResultGood);
            checkAverage += averageThrow;
            checkBestThrowAverage += 180;

            IPlayerStats better = player.GetPlayerStats();
            Assert.That(better.AverageThrow, Is.EqualTo(checkAverage / 4.0), "Average not calculated correctly.");
            Assert.That(better.AverageBestThrow, Is.EqualTo(checkBestThrowAverage / 4.0), "Best throw average not calculated correctly.");
            Assert.That(better.Total180, Is.EqualTo(3), "There should be 3 times a 180 better case.");
            Assert.That(better.BestThrow, Is.EqualTo(180), "The best is 180 in the better case.");
        }

        private IPlayer CreatePlayer(string? name)
        {
            ConstructorInfo? constructor = _playerType
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .FirstOrDefault(c => c.IsAssembly || c.IsPublic);

            Assert.That(constructor, Is.Not.Null, "Cannot find a non-private constructor.");
            ParameterInfo[] parameters = constructor!.GetParameters();
            Assert.That(parameters.Length, Is.EqualTo(1), "Cannot find a constructor that accepts 1 parameters");

            Assert.That(parameters[0].ParameterType, Is.EqualTo(typeof(string)), "The parameter should be a string (name).");

            try
            {
                return (IPlayer) constructor.Invoke(new object[] { name });
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException!;
            }
        }
    }
}
