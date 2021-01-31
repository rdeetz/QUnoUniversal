// <copyright file="MainViewModel.cs" company="Mooville">
//   Copyright © 2021 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Mooville.QUno.Model;
    using Mooville.QUno.ViewModel;

    public class LogEntry
    {
        public string Message { get; set; }
        public int Index { get; set; }
    }

    public enum LogMode
    {
        Play,
        WildPlay,
        Draw
    }

    public class MainViewModel : ViewModelBase
    {
        private Game game;
        private ObservableCollection<Player> computerPlayers;
        private Player humanPlayer;
        private string title;
        private bool isGameNotInProgress;
        private Player winner;
        private int logIndex;
        private ObservableCollection<LogEntry> log;
        private string logMessagePlay;
        private string logMessageWildPlay;
        private string logMessageDraw;

        public MainViewModel(string logMessagePlay, string logMessageWildPlay, string logMessageDraw)
        {
            this.computerPlayers = new ObservableCollection<Player>();
            this.isGameNotInProgress = true;
            this.logIndex = 0;
            this.log = new ObservableCollection<LogEntry>();
            this.logMessagePlay = logMessagePlay;
            this.logMessageWildPlay = logMessageWildPlay;
            this.logMessageDraw = logMessageDraw;
        }

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
                this.OnPropertyChanged("Title");
            }
        }

        public bool IsGameNotInProgress
        {
            get
            {
                return this.isGameNotInProgress;
            }

            set
            {
                this.isGameNotInProgress = value;
                this.OnPropertyChanged("IsGameNotInProgress");
            }
        }

        public Game Game
        {
            get
            {
                return this.game;
            }
        }

        public ObservableCollection<Player> ComputerPlayers
        {
            get
            {
                return this.computerPlayers;
            }
        }

        public Player HumanPlayer
        {
            get
            {
                return this.humanPlayer;
            }
        }

        public Player Winner
        {
            get
            {
                return this.winner;
            }

            set
            {
                this.winner = value;
                this.OnPropertyChanged("Winner");
            }
        }

        public ObservableCollection<LogEntry> Log
        {
            get
            {
                return this.log;
            }
        }

        public void StartGame(IEnumerable<Player> players)
        {
            this.computerPlayers.Clear();
            this.logIndex = 0;
            this.log.Clear();

            this.game = new Game();
            this.game.PlayerChanged += new EventHandler(this.Game_PlayerChanged);

            foreach (var player in players)
            {
                this.game.Players.Add(player);

                if (player.IsHuman)
                {
                    this.humanPlayer = player;
                }
                else
                {
                    this.computerPlayers.Add(player);
                }
            }

            this.game.Deal();

            this.IsGameNotInProgress = false;
            this.Winner = null;

            return;
        }

        public void OpenGame(Game game)
        {
            if (this.game != null)
            {
                this.game.PlayerChanged -= new EventHandler(this.Game_PlayerChanged);
                this.game = null;
            }

            this.computerPlayers.Clear();
            this.logIndex = 0;
            this.log.Clear();

            this.game = game;
            this.game.PlayerChanged += new EventHandler(this.Game_PlayerChanged);

            foreach (var player in this.game.Players)
            {
                if (player.IsHuman)
                {
                    this.humanPlayer = player;
                }
                else
                {
                    this.computerPlayers.Add(player);
                }
            }

            this.IsGameNotInProgress = false;
            this.Winner = null;

            this.OnPropertyChanged(String.Empty);

            return;
        }

        public bool CanPlayCard(Card card)
        {
            bool canPlay = false;

            if (card != null)
            {
                canPlay = this.game.CanPlay(card);
            }

            return canPlay;
        }

        public void PlayCard(Card card, Color? wildColor)
        {
            Player player = this.game.CurrentPlayer;

            if (wildColor.HasValue)
            {
                this.LogTurn(player, card, wildColor, LogMode.WildPlay);
            }
            else
            {
                this.LogTurn(player, card, null, LogMode.Play);
            }
            
            player.Hand.Cards.Remove(card);
            this.game.PlayCard(card, wildColor);

            return;
        }

        public void DrawCard()
        {
            Player player = this.game.CurrentPlayer;

            this.LogTurn(player, null, null, LogMode.Draw); // I don't like passing null for the card here, but it's easier for now.
            Card card = this.game.DrawCard();
            player.Hand.Cards.Add(card);

            return;
        }

        private void Game_PlayerChanged(object sender, EventArgs e)
        {
            // This is the line of code that make this work, 
            // even though the Game object does not raise 
            // property changed notifications. 
            // In Windows Presentation Foundation this was ignored, 
            // but in Universal Windows Platform the XAML compiler
            // raises warnings in the GameTemplate.
            this.OnPropertyChanged(String.Empty);

            if (!this.game.IsGameOver)
            {
                Player player = this.game.CurrentPlayer;

                if (!player.IsHuman)
                {
                    Card cardToPlay = player.ChooseCardToPlay(this.game);

                    if (cardToPlay != null)
                    {
                        if (cardToPlay.Color == Color.Wild)
                        {
                            Color wildColor = player.ChooseWildColor();
                            this.LogTurn(player, cardToPlay, wildColor, LogMode.WildPlay);
                            this.game.PlayCard(cardToPlay, wildColor);
                        }
                        else
                        {
                            this.LogTurn(player, cardToPlay, null, LogMode.Play);
                            this.game.PlayCard(cardToPlay);
                        }
                    }
                    else
                    {
                        this.LogTurn(player, null, null, LogMode.Draw);
                        Card cardToDraw = this.game.DrawCard();
                        player.Hand.Cards.Add(cardToDraw);
                    }
                }
            }
            else
            {
                this.IsGameNotInProgress = true;
                this.Winner = this.game.Players.FirstOrDefault(p => p.Hand.Cards.Count == 0);
            }

            return;
        }

        private void LogTurn(Player player, Card card, Color? wildColor, LogMode mode)
        {
            string message = String.Empty;

            switch (mode)
            {
                case LogMode.Play:
                    message = String.Format(this.logMessagePlay, player.Name, card.ToString());
                    break;

                case LogMode.WildPlay:
                    message = String.Format(this.logMessageWildPlay, player.Name, card.Value.ToString(), wildColor.ToString());
                    break;

                case LogMode.Draw:
                    message = String.Format(this.logMessageDraw, player.Name);
                    break;
            }

            this.logIndex++;
            LogEntry logEntry = new LogEntry() { Message = message, Index = this.logIndex };
            this.log.Add(logEntry);

            return;
        }
    }
}
