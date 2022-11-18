using DartApp.Domain.Contracts;

namespace DartApp.Tests.Builders
{
    internal class GameResultBuilder
    {
        private readonly TestGameResult _gameResult;
        public GameResultBuilder()
        {
            _gameResult = new TestGameResult()
            {
                Id = Guid.NewGuid(),
                AverageThrow = 0,
                NumberOf180 = 0,
                BestThrow = 0
            };
        }
        public GameResultBuilder WithPlayerId(Guid playerId)
        {
            _gameResult.PlayerId = playerId;
            return this;
        }

        public IGameResult Build()
        {
            return _gameResult;
        }
    }
}