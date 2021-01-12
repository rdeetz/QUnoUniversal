// <copyright file="IGameSerializer2.cs" company="Mooville">
//   Copyright © 2020 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal.Model
{
    using System.Threading.Tasks;
    using Windows.Storage;
    using Mooville.QUno.Model;

    /// <summary>
    /// Defines methods for serializing a <see cref="Game"/> object.
    /// </summary>
    public interface IGameSerializer2
    {
        /// <summary>
        /// Loads a <see cref="Game"/> from the specified file.
        /// </summary>
        /// <param name="file">
        /// The file to load.
        /// </param>
        /// <returns>
        /// A <see cref="Game"/> object initialized with data from the specified file.
        /// </returns>
        Task<Game> LoadFromFileAsync(IStorageFile file);

        /// <summary>
        /// Saves a <see cref="Game"/> to the specified file.
        /// </summary>
        /// <param name="game">
        /// The game to save.
        /// </param>
        /// <param name="file">
        /// The file to save.
        /// </param>
        void SaveToFileAsync(Game game, IStorageFile file);
    }
}
