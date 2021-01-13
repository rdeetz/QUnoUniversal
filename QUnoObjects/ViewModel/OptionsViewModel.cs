// <copyright file="OptionsViewModel.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.ViewModel
{
    /// <summary>
    /// Represents the options view.
    /// </summary>
    public class OptionsViewModel : ViewModelBase
    {
        private const int MinimumComputerPlayersAllowed = 1;
        private const int MaximumComputerPlayersAllowed = 7;

        private ISettingsProvider settingsProvider;

        private string defaultHumanPlayerName;
        private int defaultComputerPlayers;
        private int minimumComputerPlayers;
        private int maximumComputerPlayers;
        private bool autoCheckForUpdates;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsViewModel"/> class.
        /// </summary>
        /// <param name="settingsProvider">
        /// The settings provider.
        /// </param>
        public OptionsViewModel(ISettingsProvider settingsProvider)
        {
            this.settingsProvider = settingsProvider;
        }

        /// <summary>
        /// Gets or sets the default name of the human player.
        /// </summary>
        /// <value>
        /// The default name of the human player.
        /// </value>
        public string DefaultHumanPlayerName
        {
            get
            {
                return this.defaultHumanPlayerName;
            }

            set
            {
                this.defaultHumanPlayerName = value;
                this.OnPropertyChanged("DefaultHumanPlayerName");
            }
        }

        /// <summary>
        /// Gets or sets the default number of computer players.
        /// </summary>
        /// <value>
        /// The default number of computer players.
        /// </value>
        public int DefaultComputerPlayers
        {
            get
            {
                return this.defaultComputerPlayers;
            }

            set
            {
                this.defaultComputerPlayers = value;
                this.OnPropertyChanged("DefaultComputerPlayers");
            }
        }

        /// <summary>
        /// Gets the minimum number of computer players.
        /// </summary>
        /// <value>
        /// The minimum number of computer players.
        /// </value>
        public int MinimumComputerPlayers
        {
            get
            {
                return this.minimumComputerPlayers;
            }
        }

        /// <summary>
        /// Gets the maximum number of computer players.
        /// </summary>
        /// <value>
        /// The maximum number of computer players.
        /// </value>
        public int MaximumComputerPlayers
        {
            get
            {
                return this.maximumComputerPlayers;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the application should automatically check for updates.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the application should automatically check for updates; otherwise, <see langword="false"/>.
        /// </value>
        public bool AutoCheckForUpdates
        {
            get
            {
                return this.autoCheckForUpdates;
            }

            set
            {
                this.autoCheckForUpdates = value;
                this.OnPropertyChanged("AutoCheckForUpdates");
            }
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        public void LoadSettings()
        {
            this.settingsProvider.LoadSettings();

            this.defaultHumanPlayerName = this.settingsProvider.DefaultHumanPlayerName;
            this.defaultComputerPlayers = this.settingsProvider.DefaultComputerPlayers;
            this.minimumComputerPlayers = OptionsViewModel.MinimumComputerPlayersAllowed;
            this.maximumComputerPlayers = OptionsViewModel.MaximumComputerPlayersAllowed;
            this.autoCheckForUpdates = this.settingsProvider.AutoCheckForUpdates;

            return;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveSettings()
        {
            this.settingsProvider.DefaultHumanPlayerName = this.defaultHumanPlayerName;
            this.settingsProvider.DefaultComputerPlayers = this.defaultComputerPlayers;
            this.settingsProvider.AutoCheckForUpdates = this.autoCheckForUpdates;

            this.settingsProvider.SaveSettings();

            return;
        }
    }
}
