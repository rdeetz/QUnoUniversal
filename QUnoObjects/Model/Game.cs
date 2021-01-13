// <copyright file="Game.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a game.
    /// </summary>
    public class Game
    {
        private List<Player> players;
        private Deck deck;
        private Direction currentDirection;
        private int currentPlayerIndex;
        private bool neededToReshuffleDuringDeal;
        private IRuleProvider ruleProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        public Game()
            : this(new Deck(), new StandardRuleProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="deck">
        /// The deck to use in this game.
        /// </param>
        /// <param name="ruleProvider">
        /// The rule provider.
        /// </param>
        public Game(Deck deck, IRuleProvider ruleProvider)
        {
            if (deck == null)
            {
                throw new ArgumentNullException("deck");
            }

            if (ruleProvider == null)
            {
                throw new ArgumentNullException("ruleProvider");
            }

            this.players = new List<Player>();
            this.deck = deck;
            this.deck.Reshuffle += new EventHandler(this.Deck_Reshuffle);
            this.ruleProvider = ruleProvider;
        }

        /// <summary>
        /// Occurs when the current player changes.
        /// </summary>
        public event EventHandler PlayerChanged;

        /// <summary>
        /// Gets the players in the game.
        /// </summary>
        /// <value>
        /// The players in the game.
        /// </value>
        public IList<Player> Players
        {
            get
            {
                return this.players;
            }
        }

        /// <summary>
        /// Gets the deck in the game.
        /// </summary>
        /// <value>
        /// The deck in the game.
        /// </value>
        public Deck Deck
        {
            get
            {
                return this.deck;
            }
        }

        /// <summary>
        /// Gets the current direction of play.
        /// </summary>
        /// <value>
        /// The current direction of play.
        /// </value>
        public Direction CurrentDirection
        {
            get
            {
                return this.currentDirection;
            }

            set
            {
                this.currentDirection = value;
            }
        }

        /// <summary>
        /// Gets the current player.
        /// </summary>
        /// <value>
        /// The current player.
        /// </value>
        public Player CurrentPlayer
        {
            get
            {
                Player player = null;

                if (this.players.Count > 0)
                {
                    try
                    {
                        player = this.players[this.currentPlayerIndex];
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // Just allow this property to return null.
                    }
                }

                return player;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the game is over.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this game is over; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsGameOver
        {
            get
            {
                bool gameOver = false;

                foreach (var player in this.Players)
                {
                    if (player.Hand.Cards.Count == 0)
                    {
                        gameOver = true;
                        break;
                    }
                }

                return gameOver;
            }
        }

        /// <summary>
        /// Gets or sets the index of the current player.
        /// </summary>
        /// <value>
        /// The index of the current player.
        /// </value>
        public int CurrentPlayerIndex
        {
            get
            {
                return this.currentPlayerIndex;
            }

            set
            {
                this.currentPlayerIndex = value;
            }
        }

        /// <summary>
        /// Deals the cards before the game starts.
        /// </summary>
        public void Deal()
        {
            // Make sure there are players in this game.
            if (this.players.Count <= 0)
            {
                throw new InvalidOperationException("There are no players in the game.");
            }

            // Make sure the deck has enough cards for this number of players.
            // Right now we just require a buffer of +1 (for the current card)
            // but we may need to increase this as we experiment.
            int cardsNeeded = (this.ruleProvider.HandSize * this.players.Count) + 1;

            if (cardsNeeded > this.deck.DrawPile.Count)
            {
                // I'm not sure InvalidOperationException is the right type to throw here.
                // Additionally, the game should probably have a way to limit
                // the number of players and/or suggest how many cards should be
                // initially in each hand for a given number of players.
                throw new InvalidOperationException("There are not enough cards in the deck for this game.");
            }

            // Shuffle up and deal.
            this.deck.Shuffle();

            for (int i = 0; i < this.ruleProvider.HandSize; i++)
            {
                foreach (Player player in this.players)
                {
                    player.Hand.Cards.Add(this.deck.Draw());
                }
            }

            // Set the current card.
            // It cannot be wild, but that could be configurable.
            // This needs to handle the situation where there 
            // are only wild cards left in the deck, to avoid infinite loop.
            Card currentCard;

            do
            {
                currentCard = this.deck.Draw();
                this.deck.Play(currentCard);
            }
            while ((this.deck.CurrentCard.Color == Color.Wild) && (!this.neededToReshuffleDuringDeal));

            if (this.neededToReshuffleDuringDeal)
            {
                // TODO Take corrective action--redeal all the cards, or force somebody to choose a wild color start.
            }

            this.OnPlayerChanged();

            return;
        }

        /// <summary>
        /// Determines whether the given card can be played on the current card.
        /// </summary>
        /// <param name="card">
        /// The card to play.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this card can be played; otherwise, <see langword="false"/>.
        /// </returns>
        public bool CanPlay(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException("card");
            }

            Card currentCard = this.deck.CurrentCard;

            if (currentCard == null)
            {
                throw new InvalidOperationException("There is no current card. Have the cards been dealt?");
            }

            bool canPlay = this.ruleProvider.CanPlay(card, currentCard, this.deck.CurrentWildColor);

            return canPlay;
        }

        /// <summary>
        /// Plays the card.
        /// </summary>
        /// <param name="card">
        /// The card to be played.
        /// </param>
        public void PlayCard(Card card)
        {
            this.PlayCard(card, null);

            return;
        }

        /// <summary>
        /// Plays the card.
        /// </summary>
        /// <param name="card">
        /// The card to be played.
        /// </param>
        /// <param name="wildColor">
        /// The wild color to apply, or <see langword="null"/>.
        /// </param>
        public void PlayCard(Card card, Color? wildColor)
        {
            if (card == null)
            {
                throw new ArgumentNullException("card");
            }

            if (this.players.Count <= 0)
            {
                throw new InvalidOperationException("There are no players in the game.");
            }

            if (!this.CanPlay(card))
            {
                throw new InvalidOperationException("This card cannot be played right now.");
            }

            if (!this.ruleProvider.ValidateWildColor(card, wildColor))
            {
                throw new InvalidOperationException("A valid color must be provided when playing a wild card.");
            }

            // Play the given card.
            this.deck.Play(card);

            // Update the current wild color.
            if (card.Color == Color.Wild)
            {
                this.deck.CurrentWildColor = wildColor;
            }
            else
            {
                this.deck.CurrentWildColor = null;
            }

            // See if the card changes the direction.
            bool changedDirection = this.ruleProvider.ChangedDirection(card);

            if (changedDirection)
            {
                if (this.currentDirection == Direction.Clockwise)
                {
                    this.currentDirection = Direction.Counterclockwise;
                }
                else
                {
                    this.currentDirection = Direction.Clockwise;
                }
            }

            // See how many players the card increments.
            int playerIncrement = this.ruleProvider.GetPlayerIncrement(card);

            for (int i = 0; i < playerIncrement; i++)
            {
                this.SelectNextPlayer();
            }

            // See how many cards need to be added to the next player's hand.
            int numberOfCardsToAdd = this.ruleProvider.GetNumberOfCardsToAdd(card);

            if (numberOfCardsToAdd > 0)
            {
                for (int i = 0; i < numberOfCardsToAdd; i++)
                {
                    Card cardToDraw = this.deck.Draw();
                    this.CurrentPlayer.Hand.Cards.Add(cardToDraw);
                }

                // If a particular card causes a draw event, 
                // we assume that they will be "skipped", so 
                // move on to the next player.
                this.SelectNextPlayer();
            }

            this.OnPlayerChanged();

            return;
        }

        /// <summary>
        /// Draws a card and selects the next player. Use method when a player has no cards in their hand that they can play.
        /// </summary>
        /// <returns>
        /// A <see cref="Card"/> from the top of the draw pile.
        /// </returns>
        public Card DrawCard()
        {
            Card card = this.deck.Draw();
            this.SelectNextPlayer();

            this.OnPlayerChanged();

            return card;
        }

        /// <summary>
        /// Called when the current player changes.
        /// </summary>
        protected void OnPlayerChanged()
        {
            EventHandler tmp = this.PlayerChanged;

            if (tmp != null)
            {
                tmp(this, new EventArgs());
            }

            return;
        }

        private void SelectNextPlayer()
        {
            // Select the next player, but be sure to "wrap around" the index if needed.
            if (this.currentDirection == Direction.Clockwise)
            {
                this.currentPlayerIndex++;

                if (this.currentPlayerIndex > (this.players.Count - 1))
                {
                    this.currentPlayerIndex = 0;
                }
            }
            else
            {
                this.currentPlayerIndex--;

                if (this.currentPlayerIndex < 0)
                {
                    this.currentPlayerIndex = this.players.Count - 1;
                }
            }

            return;
        }

        private void Deck_Reshuffle(object sender, EventArgs e)
        {
            this.neededToReshuffleDuringDeal = true;

            return;
        }
    }
}
