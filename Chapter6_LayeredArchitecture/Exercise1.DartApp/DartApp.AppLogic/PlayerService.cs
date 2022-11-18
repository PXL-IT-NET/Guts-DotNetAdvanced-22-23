using DartApp.AppLogic.Contracts;
using DartApp.Domain;
using DartApp.Domain.Contracts;
using System.Collections.Generic;

namespace DartApp.AppLogic
{
    public class PlayerService
    {
        public PlayerService()
        {
               
        }

        public void AddGameResultForPlayer(IPlayer player, int number180, double average, int bestThrow)
        {
            throw new System.NotImplementedException();
        }

        public IPlayer AddPlayer(string playerName)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyList<IPlayer> GetAllPlayers()
        {
            throw new System.NotImplementedException();
        }

        public IPlayerStats GetStatsForPlayer(IPlayer player)
        {
            throw new System.NotImplementedException();
        }
    }
}
