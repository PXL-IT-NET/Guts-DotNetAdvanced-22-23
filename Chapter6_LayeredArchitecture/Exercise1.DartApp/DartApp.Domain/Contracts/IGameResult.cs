using System;

namespace DartApp.Domain.Contracts
{
    public interface IGameResult
    {
        Guid Id { get; }
        Guid PlayerId { get; }
        int NumberOf180 { get; }
        double AverageThrow { get; }
        int BestThrow { get; }
    }
}
