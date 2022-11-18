using DartApp.Domain.Contracts;
using System.Collections.Generic;

namespace DartApp.AppLogic.Contracts
{
    public interface IPlayerService
    {
        IPlayer AddPlayer(string playerName);
        void AddGameResultForPlayer(IPlayer player, int number180, double average, int bestThrow);
        IPlayerStats GetStatsForPlayer(IPlayer player);
        IReadOnlyList<IPlayer> GetAllPlayers();

    }
}
