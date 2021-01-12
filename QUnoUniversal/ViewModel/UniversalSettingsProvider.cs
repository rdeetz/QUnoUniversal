// <copyright file="UniversalSettingsProvider.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal.ViewModel
{
    using System;
    using Mooville.QUno.ViewModel;
    using Windows.Storage;

    public class UniversalSettingsProvider : ISettingsProvider
    {
        private const string DefaultHumanPlayerNameKey = @"DefaultHumanPlayerName";
        private const string DefaultComputerPlayersKey = @"DefaultComputerPlayers";
        private const string UniversalDefaultHumanPlayerName = @"Player 1";
        private const int UniversalDefaultComputerPlayers = 3;

        private string defaultHumanPlayerName;
        private int defaultComputerPlayers;

        public UniversalSettingsProvider()
        {
        }

        #region ISettingsProvider Members

        public string DefaultHumanPlayerName
        {
            get
            {
                return this.defaultHumanPlayerName;
            }

            set
            {
                this.defaultHumanPlayerName = value;
            }
        }

        public int DefaultComputerPlayers
        {
            get
            {
                return this.defaultComputerPlayers;
            }

            set
            {
                this.defaultComputerPlayers = value;
            }
        }

        public bool AutoCheckForUpdates
        {
            get
            {
                return false;
            }

            set
            {
                return;
            }
        }

        public void LoadSettings()
        {
            this.defaultHumanPlayerName = this.GetValue<string>(UniversalSettingsProvider.DefaultHumanPlayerNameKey, UniversalSettingsProvider.UniversalDefaultHumanPlayerName);
            this.defaultComputerPlayers = this.GetValue<int>(UniversalSettingsProvider.DefaultComputerPlayersKey, UniversalSettingsProvider.UniversalDefaultComputerPlayers);

            return;
        }

        public void SaveSettings()
        {
            this.SetValue(UniversalSettingsProvider.DefaultHumanPlayerNameKey, this.defaultHumanPlayerName);
            this.SetValue(UniversalSettingsProvider.DefaultComputerPlayersKey, this.defaultComputerPlayers);

            return;
        }

        #endregion

        private T GetValue<T>(string key, T defaultValue)
        {
            object value = ApplicationData.Current.RoamingSettings.Values[key];

            if (value != null)
            {
                return (T)value;
            }
            else
            {
                return defaultValue;
            }
        }

        private void SetValue(string key, object value)
        {
            ApplicationData.Current.RoamingSettings.Values[key] = value;

            return;
        }
    }
}
