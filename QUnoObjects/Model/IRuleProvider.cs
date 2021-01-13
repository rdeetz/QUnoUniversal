// <copyright file="IRuleProvider.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Model
{
    /// <summary>
    /// Provides rules for a <see cref="Game"/>.
    /// </summary>
    public interface IRuleProvider
    {
        /// <summary>
        /// Gets the size of the hand.
        /// </summary>
        /// <value>
        /// The size of the hand.
        /// </value>
        int HandSize { get; }

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
        bool CanPlay(Card card, Card currentCard, Color? currentWildColor);

        /// <summary>
        /// Determines whether the given card changed the direction of play.
        /// </summary>
        /// <param name="card">
        /// The card that was played.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if this card changed the direction of play; otherwise, <see langword="false"/>.
        /// </returns>
        bool ChangedDirection(Card card);

        /// <summary>
        /// Determines how many players the given card increments.
        /// </summary>
        /// <param name="card">
        /// The card that was played.
        /// </param>
        /// <returns>
        /// The number of players to advance.
        /// </returns>
        int GetPlayerIncrement(Card card);

        /// <summary>
        /// Determines how many cards should be added to the next player given this card.
        /// </summary>
        /// <param name="card">
        /// The card that was played.
        /// </param>
        /// <returns>
        /// The number of cards the next player needs to draw.
        /// </returns>
        int GetNumberOfCardsToAdd(Card card);

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
        bool ValidateWildColor(Card card, Color? wildColor);
    }
}
