using DartApp.Domain.Contracts;
using System.Collections.Generic;

namespace DartApp.AppLogic.Contracts
{
    public interface IPlayerRepository
    {
        void Add(IPlayer player);
        IReadOnlyList<IPlayer> GetAll();
        void SaveChanges(IPlayer player);
    }

}
