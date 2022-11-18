using DartApp.AppLogic.Contracts;
using DartApp.Domain;
using DartApp.Domain.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DartApp.Infrastructure.Storage
{
    public class PlayerFileRepository
    {
        private readonly string _playerFileDirectory;

        public PlayerFileRepository(string playerFileDirectory)
        {
            
        }

        public void Add(IPlayer player)
        {
            SavePlayer(player);
        }

        public IReadOnlyList<IPlayer> GetAll()
        {
            //TODO: read all player files in the directory, convert them to IPlayer objects and return them
            //Tip: use helper methods that are given (ReadPlayerloadFromFile)
            return null;
        }

        public void SaveChanges(IPlayer player)
        {
            SavePlayer(player);
        }


        private IPlayer ReadPlayerFromFile(string playerFilePath)
        {
            //TODO: read the json in a player file and deserialize the json into an IPlayer object
            //Tip: use helper methods that are given (ConvertJsonToPlayer)
            return null;
        }

        private void SavePlayer(IPlayer player)
        {
            //TODO: save the player in a json format in a file
            //Tip: use helper methods that are given (GetPLayerilePath, ConvertPlayerToJson)
        }

        private string ConvertPlayerToJson(IPlayer player)
        {
            string json = JsonConvert.SerializeObject(player, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return json;
        }

        private IPlayer ConvertJsonToPlayer(string json)
        {
            return JsonConvert.DeserializeObject<Player>(json, new JsonSerializerSettings
            {
                ContractResolver = new JsonAllowPrivateSetterContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                TypeNameHandling = TypeNameHandling.Auto
            }) as IPlayer;
        }

        private string GetPlayerFilePath(Guid playerId)
        {
            string fileName = $"Player_{playerId}.json";
            return Path.Combine(_playerFileDirectory, fileName);
        }
    }
}
