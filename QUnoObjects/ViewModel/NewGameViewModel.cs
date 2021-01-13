// <copyright file="NewGameViewModel.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Mooville.QUno.Model;

    /// <summary>
    /// Represents the new game view.
    /// </summary>
    public abstract class NewGameViewModel : ViewModelBase
    {
        private string humanPlayerName;
        private string playerNameTemplate;
        private ObservableCollection<Player> computerPlayersCollection;
        ////private int maxPlayers = 6; // TODO Get this from the rule provider?

        /// <summary>
        /// Initializes a new instance of the <see cref="NewGameViewModel" /> class.
        /// </summary>
        /// <param name="humanPlayerName">
        /// Name of the human player.
        /// </param>
        /// <param name="numberOfComputerPlayers">
        /// The number of computer players.
        /// </param>
        /// <param name="playerNameTemplate">
        /// The player name template.
        /// </param>
        public NewGameViewModel(string humanPlayerName, int numberOfComputerPlayers, string playerNameTemplate)
        {
            this.humanPlayerName = humanPlayerName;
            this.playerNameTemplate = playerNameTemplate;
            this.computerPlayersCollection = new ObservableCollection<Player>();

            int playerIndex = 2; // Start at 2 since there is a human player.

            for (int i = 0; i < numberOfComputerPlayers; i++)
            {
                string playerName = String.Format(this.playerNameTemplate, playerIndex);
                this.computerPlayersCollection.Add(new Player() { Name = playerName, IsHuman = false });
                playerIndex++;
            }
        }

        #region Public Properties

        /// <summary>
        /// Gets the players.
        /// </summary>
        /// <value>
        /// The players.
        /// </value>
        public IEnumerable<Player> Players
        {
            get
            {
                List<Player> players = new List<Player>();

                Player humanPlayer = new Player() { Name = this.humanPlayerName, IsHuman = true };
                players.Add(humanPlayer);

                foreach (var computerPlayer in this.computerPlayersCollection)
                {
                    players.Add(computerPlayer);
                }

                return players;
            }
        }

        /// <summary>
        /// Gets or sets the name of the human player.
        /// </summary>
        /// <value>
        /// The name of the human player.
        /// </value>
        public string HumanPlayerName
        {
            get
            {
                return this.humanPlayerName;
            }

            set
            {
                this.humanPlayerName = value;
                this.OnPropertyChanged("HumanPlayerName");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can add computer player.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this instance can add computer player; otherwise, <see langword="false"/>.
        /// </value>
        public virtual bool CanAddComputerPlayer
        {
            get
            {
                // TODO Should check to see if any more players are allowed in this game.
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can remove computer player.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this instance can remove computer player; otherwise, <see langword="false"/>.
        /// </value>
        public virtual bool CanRemoveComputerPlayer
        {
            get
            {
                // TODO Should check to see if the minimum players are met.
                return this.computerPlayersCollection.Count > 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can start game.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this instance can start game; otherwise, <see langword="false"/>.
        /// </value>
        public virtual bool CanStartGame
        {
            get
            {
                // TODO Should check if there are too many players in this game.
                return !String.IsNullOrWhiteSpace(this.humanPlayerName) && (this.computerPlayersCollection.Count >= 1);
            }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets the player name template.
        /// </summary>
        /// <value>
        /// The player name template.
        /// </value>
        protected string PlayerNameTemplate
        {
            get
            {
                return this.playerNameTemplate;
            }
        }

        /// <summary>
        /// Gets the computer players collection.
        /// </summary>
        /// <value>
        /// The computer players collection.
        /// </value>
        protected ObservableCollection<Player> ComputerPlayersCollection
        {
            get
            {
                return this.computerPlayersCollection;
            }
        }

        #endregion

        /// <summary>
        /// Adds a computer player.
        /// </summary>
        public void AddComputerPlayer()
        {
            // TODO This could create a name collision, so we should search to see if the computed name exists.
            int playerIndex = this.computerPlayersCollection.Count + 2; // Add two since there is a human player.
            string playerName = String.Format(this.playerNameTemplate, playerIndex);
            Player newPlayer = new Player() { Name = playerName, IsHuman = false };
            this.computerPlayersCollection.Add(newPlayer);

            return;
        }

        /// <summary>
        /// Removes the computer player.
        /// </summary>
        /// <param name="player">
        /// The player to remove.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// The player to remove is null.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// There are no computer players to remove.
        /// </exception>
        public void RemoveComputerPlayer(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException("player");
            }

            if (this.computerPlayersCollection.Count <= 0)
            {
                throw new InvalidOperationException("There are no computer players to remove.");
            }

            this.computerPlayersCollection.Remove(player);

            return;
        }
    }
}
