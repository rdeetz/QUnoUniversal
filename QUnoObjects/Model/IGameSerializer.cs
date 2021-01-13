// <copyright file="IGameSerializer.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Model
{
    /// <summary>
    /// Defines methods for serializing a <see cref="Game"/> object.
    /// </summary>
    public interface IGameSerializer
    {
        /// <summary>
        /// Loads a <see cref="Game"/> from the specified file name.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file to load.
        /// </param>
        /// <returns>
        /// A <see cref="Game"/> object initialized with data from the specified file name.
        /// </returns>
        Game Load(string fileName);

        /// <summary>
        /// Saves a <see cref="Game"/> to the specified file name.
        /// </summary>
        /// <param name="game">
        /// The game to save.
        /// </param>
        /// <param name="fileName">
        /// The name of the file to save.
        /// </param>
        void Save(Game game, string fileName);
    }
}
