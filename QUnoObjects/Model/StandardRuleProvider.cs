// <copyright file="StandardRuleProvider.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Model
{
    using System;

    /// <summary>
    /// Represents a standard implementation of <see cref="IRuleProvider"/>.
    /// </summary>
    public class StandardRuleProvider : IRuleProvider
    {
        /// <summary>
        /// The size of a standard hand.
        /// </summary>
        public const int StandardHandSize = 5;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardRuleProvider"/> class.
        /// </summary>
        public StandardRuleProvider()
        {
        }

        #region IRuleProvider Members

        /// <summary>
        /// Gets the size of the hand.
        /// </summary>
        /// <value>
        /// The size of the hand.
        /// </value>
        public int HandSize
        {
            get
            {
                return StandardRuleProvider.StandardHandSize;
            }
        }

        /// <summary>
        /// Determines whether the given card can be played on the current card.
        /// </summary>
        /// <param name="card">
        /// The card to play.
        /// </param>
        /// <param name="currentCard">
        /// The current card.
        /// </param>
        /// <param name="currentWildColor">
        /// Color of the current wild card, or <see langword="null"/> if the current card is not wild.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this card can be played; otherwise, <see langword="false"/>.
        /// </returns>
        public bool CanPlay(Card card, Card currentCard, Color? currentWildColor)
        {
            if (card == null)
            {
                throw new ArgumentNullException("card");
            }

            if (currentCard == null)
            {
                throw new ArgumentNullException("currentCard");
            }

            if ((currentCard.Color == Color.Wild) && (currentWildColor == null))
            {
                throw new ArgumentNullException("currentWildColor", "CurrentCard is Wild, but CurrentWildColor is null.");
            }

            bool canPlay = false;

            if (card.Color == Color.Wild)
            {
                canPlay = true;
            }
            else if (card.Color == currentCard.Color)
            {
                canPlay = true;
            }
            else if (card.Value == currentCard.Value)
            {
                canPlay = true;
            }
            else if (currentCard.Color == Color.Wild)
            {
                if (card.Color == currentWildColor)
                {
                    canPlay = true;
                }
            }

            return canPlay;
        }

        /// <summary>
        /// Determines whether the given card changed the direction of play.
        /// </summary>
        /// <param name="card">
        /// The card that was played.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this card changed the direction of play; otherwise, <see langword="false"/>.
        /// </returns>
        public bool ChangedDirection(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException("card");
            }

            bool changedDirection = false;

            if (card.Value == Value.Reverse)
            {
                changedDirection = true;
            }

            return changedDirection;
        }

        /// <summary>
        /// Determines how many players the given card increments.
        /// </summary>
        /// <param name="card">
        /// The card that was played.
        /// </param>
        /// <returns>
        /// The number of players to advance.
        /// </returns>
        public int GetPlayerIncrement(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException("card");
            }

            int defaultIncrement = 1;

            if (card.Value == Value.Skip)
            {
                defaultIncrement = 2;
            }

            return defaultIncrement;
        }

        /// <summary>
        /// Determines how many cards should be added to the next player given this card.
        /// </summary>
        /// <param name="card">
        /// The card that was played.
        /// </param>
        /// <returns>
        /// The number of cards the next player needs to draw.
        /// </returns>
        public int GetNumberOfCardsToAdd(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException("card");
            }

            int numberOfCardsToAdd = 0;

            if (card.Value == Value.DrawTwo)
            {
                numberOfCardsToAdd = 2;
            }
            else if (card.Value == Value.WildDrawFour)
            {
                numberOfCardsToAdd = 4;
            }

            return numberOfCardsToAdd;
        }

        /// <summary>
        /// Determines whether a given card and wild color are valid.
        /// </summary>
        /// <param name="card">
        /// The card to be played.
        /// </param>
        /// <param name="wildColor">
        /// The wild color to go with the card.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this card and wild color combination are valid; otherwise, <see langword="false"/>.
        /// </returns>
        public bool ValidateWildColor(Card card, Color? wildColor)
        {
            if (card == null)
            {
                throw new ArgumentNullException("card");
            }

            bool result = true;

            if (card.Color == Color.Wild)
            {
                if (wildColor == null)
                {
                    // If they provided a wild card, then they must also provide a wild color.
                    result = false;
                }

                if (wildColor == Color.Wild)
                {
                    // If they provided a wild card, the wild color must not be Wild.
                    result = false;
                }
            }

            return result;
        }

        #endregion
    }
}
