using DartApp.Domain.Contracts;

namespace DartApp.Tests.Builders
{
    internal class PlayerBuilder
    {
        protected static Random Random = new Random();
        private readonly TestPlayer _player;

        public PlayerBuilder(bool createGameResults = true)
        {
            List<IGameResult> gameResults = new List<IGameResult>();
            _player = new TestPlayer
            {
                Id = Guid.NewGuid(),
                GameResults = gameResults,
                Name = Guid.NewGuid().ToString()
            };
            if (createGameResults)
            {
                for (int i = 0; i < Random.Next(1, 5); i++)
                {
                    gameResults.Add(new GameResultBuilder().WithPlayerId(_player.Id).Build());
                }
            }
        }

        public PlayerBuilder WithName(string name)
        {
            _player.Name = name;
            return this;
        }

        public IPlayer Build()
        {
            return _player;
        }
    }
}
