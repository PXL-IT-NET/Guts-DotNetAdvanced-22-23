using DartApp.Domain.Contracts;

namespace DartApp.Tests
{
    internal class TestGameResult : IGameResult
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }

        public int NumberOf180 { get; set; }

        public double AverageThrow { get; set; }

        public int BestThrow { get; set; }
    }
}
