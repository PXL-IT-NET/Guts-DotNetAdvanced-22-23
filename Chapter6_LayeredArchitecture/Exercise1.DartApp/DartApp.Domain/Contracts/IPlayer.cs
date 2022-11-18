using System;
using System.Collections.Generic;

namespace DartApp.Domain.Contracts
{
    public interface IPlayer
    {
        Guid Id { get; }
        string Name { get; }
        IReadOnlyCollection<IGameResult> GameResults { get; }
        void AddGameResult(IGameResult gameResult);
        IPlayerStats GetPlayerStats();

    }
}
