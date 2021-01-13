// <copyright file="Player.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Model
{
    /// <summary>
    /// Represents a player.
    /// </summary>
    public class Player
    {
        private string name;
        private bool isHuman;
        private Hand hand;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        public Player()
        {
            this.hand = new Hand();
        }

        /// <summary>
        /// Gets or sets the name of this player.
        /// </summary>
        /// <value>
        /// The name of this player.
        /// </value>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is a human player.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this instance is a human player; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsHuman
        {
            get
            {
                return this.isHuman;
            }

            set
            {
                this.isHuman = value;
            }
        }

        /// <summary>
        /// Gets the <see cref="Hand"/> for this player.
        /// </summary>
        /// <value>
        /// The hand for this player.
        /// </value>
        public Hand Hand
        {
            get
            {
                return this.hand;
            }
        }

        /// <summary>
        /// Chooses a card to play from this player's hand.
        /// </summary>
        /// <param name="game">
        /// The current <see cref="Game"/> that the player is in.
        /// </param>
        /// <returns>
        /// A <see cref="Card"/> that the player has selected.
        /// </returns>
        public virtual Card ChooseCardToPlay(Game game)
        {
            Card cardToPlay = null;

            foreach (var card in this.Hand.Cards)
            {
                if (game.CanPlay(card))
                {
                    cardToPlay = card;
                    int cardToPlayIndex = this.Hand.Cards.IndexOf(card);
                    this.Hand.Cards.RemoveAt(cardToPlayIndex);
                    break;
                }
            }

            return cardToPlay;
        }

        /// <summary>
        /// Chooses the best wild color for this player's hand.
        /// </summary>
        /// <returns>
        /// A <see cref="Color"/> that the player has selected.
        /// </returns>
        public virtual Color ChooseWildColor()
        {
            Color color = Color.Red;

            foreach (var card in this.Hand.Cards)
            {
                if (card.Color != Color.Wild)
                {
                    color = card.Color;
                    break;
                }
            }

            return color;
        }
    }
}
