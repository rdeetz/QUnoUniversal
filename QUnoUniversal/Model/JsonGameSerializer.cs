// <copyright file="JsonGameSerializer.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal.Model
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;
    using Windows.Storage;
    using Mooville.QUno.Model;

    public class JsonGameSerializer : IGameSerializer, IGameSerializer2
    {
        public JsonGameSerializer()
        {
        }

        #region IGameSerializer Members

        public Game Load(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            throw new NotImplementedException("Use IGameSerializer2.LoadFromFile instead.");
        }

        public void Save(Game game, string fileName)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            if (String.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            throw new NotImplementedException("Use IGameSerializer2.SaveToFile instead.");
        }

        #endregion

        #region IGameSerializer2 Members

        public async Task<Game> LoadFromFileAsync(IStorageFile file)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            var gameString = await FileIO.ReadTextAsync(file);
            var game = JsonSerializer.Deserialize(gameString, typeof(Game), options) as Game;

            return game;
        }

        public async void SaveToFileAsync(Game game, IStorageFile file)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            var gameString = JsonSerializer.Serialize(game, typeof(Game), options);
            await FileIO.WriteTextAsync(file, gameString);

            return;
        }

        #endregion
    }
}
