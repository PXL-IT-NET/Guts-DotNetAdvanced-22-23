using DartApp.AppLogic;
using DartApp.Domain.Contracts;
using System;
using System.Collections.Generic;

namespace DartApp.Domain
{
    public class Player : IPlayer
    {
        public Guid Id => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public IReadOnlyCollection<IGameResult> GameResults => throw new NotImplementedException();

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
