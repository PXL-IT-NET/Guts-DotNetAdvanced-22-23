using DartApp.Domain.Contracts;

namespace DartApp.Tests
{
    internal class TestPlayer : IPlayer
    {
        public Guid Id { get; set; } 

        public string? Name { get; set; }

        public IReadOnlyCollection<IGameResult>? GameResults { get; set; }

        public void AddGameResult(IGameResult gameResult)
        {
            throw new NotImplementedException();
        }
        public IPlayerStats GetPlayerStats()
        {
            throw new NotImplementedException();
        }
    }
}
