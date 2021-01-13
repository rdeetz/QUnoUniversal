// <copyright file="ISettingsProvider.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.ViewModel
{
    /// <summary>
    /// Defines the settings to be provided by platform-specific implementations.
    /// </summary>
    public interface ISettingsProvider
    {
        /// <summary>
        /// Gets or sets the default name of the human player.
        /// </summary>
        /// <value>
        /// The default name of the human player.
        /// </value>
        string DefaultHumanPlayerName { get; set; }

        /// <summary>
        /// Gets or sets the default number of computer players.
        /// </summary>
        /// <value>
        /// The default number of computer players.
        /// </value>
        int DefaultComputerPlayers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the application should automatically check for updates.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the application should automatically check for updates; otherwise, <see langword="false"/>.
        /// </value>
        bool AutoCheckForUpdates { get; set; }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        void LoadSettings();

        /// <summary>
        /// Saves the settings.
        /// </summary>
        void SaveSettings();
    }
}
