// <copyright file="StandardDeckProvider.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Model
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a standard implementation of <see cref="IDeckProvider"/>.
    /// </summary>
    public class StandardDeckProvider : IDeckProvider
    {
        /// <summary>
        /// The size of a standard deck.
        /// </summary>
        public const int StandardDeckSize = 56;

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardDeckProvider"/> class.
        /// </summary>
        public StandardDeckProvider()
        {
        }

        #region IDeckProvider Members

        /// <summary>
        /// Provides the cards that will be added to the <see cref="Deck"/>.
        /// </summary>
        /// <returns>
        /// The cards to add to the <see cref="Deck"/>.
        /// </returns>
        public IEnumerable<Card> ProvideCards()
        {
            List<Card> cards = new List<Card>(StandardDeckProvider.StandardDeckSize);

            cards.Add(new Card(Color.Wild, Value.Wild));
            cards.Add(new Card(Color.Wild, Value.Wild));
            cards.Add(new Card(Color.Wild, Value.WildDrawFour));
            cards.Add(new Card(Color.Wild, Value.WildDrawFour));
            cards.Add(new Card(Color.Red, Value.DrawTwo));
            cards.Add(new Card(Color.Red, Value.Reverse));
            cards.Add(new Card(Color.Red, Value.Skip));
            cards.Add(new Card(Color.Red, Value.Zero));
            cards.Add(new Card(Color.Red, Value.One));
            cards.Add(new Card(Color.Red, Value.Two));
            cards.Add(new Card(Color.Red, Value.Three));
            cards.Add(new Card(Color.Red, Value.Four));
            cards.Add(new Card(Color.Red, Value.Five));
            cards.Add(new Card(Color.Red, Value.Six));
            cards.Add(new Card(Color.Red, Value.Seven));
            cards.Add(new Card(Color.Red, Value.Eight));
            cards.Add(new Card(Color.Red, Value.Nine));
            cards.Add(new Card(Color.Blue, Value.DrawTwo));
            cards.Add(new Card(Color.Blue, Value.Reverse));
            cards.Add(new Card(Color.Blue, Value.Skip));
            cards.Add(new Card(Color.Blue, Value.Zero));
            cards.Add(new Card(Color.Blue, Value.One));
            cards.Add(new Card(Color.Blue, Value.Two));
            cards.Add(new Card(Color.Blue, Value.Three));
            cards.Add(new Card(Color.Blue, Value.Four));
            cards.Add(new Card(Color.Blue, Value.Five));
            cards.Add(new Card(Color.Blue, Value.Six));
            cards.Add(new Card(Color.Blue, Value.Seven));
            cards.Add(new Card(Color.Blue, Value.Eight));
            cards.Add(new Card(Color.Blue, Value.Nine));
            cards.Add(new Card(Color.Yellow, Value.DrawTwo));
            cards.Add(new Card(Color.Yellow, Value.Reverse));
            cards.Add(new Card(Color.Yellow, Value.Skip));
            cards.Add(new Card(Color.Yellow, Value.Zero));
            cards.Add(new Card(Color.Yellow, Value.One));
            cards.Add(new Card(Color.Yellow, Value.Two));
            cards.Add(new Card(Color.Yellow, Value.Three));
            cards.Add(new Card(Color.Yellow, Value.Four));
            cards.Add(new Card(Color.Yellow, Value.Five));
            cards.Add(new Card(Color.Yellow, Value.Six));
            cards.Add(new Card(Color.Yellow, Value.Seven));
            cards.Add(new Card(Color.Yellow, Value.Eight));
            cards.Add(new Card(Color.Yellow, Value.Nine));
            cards.Add(new Card(Color.Green, Value.DrawTwo));
            cards.Add(new Card(Color.Green, Value.Reverse));
            cards.Add(new Card(Color.Green, Value.Skip));
            cards.Add(new Card(Color.Green, Value.Zero));
            cards.Add(new Card(Color.Green, Value.One));
            cards.Add(new Card(Color.Green, Value.Two));
            cards.Add(new Card(Color.Green, Value.Three));
            cards.Add(new Card(Color.Green, Value.Four));
            cards.Add(new Card(Color.Green, Value.Five));
            cards.Add(new Card(Color.Green, Value.Six));
            cards.Add(new Card(Color.Green, Value.Seven));
            cards.Add(new Card(Color.Green, Value.Eight));
            cards.Add(new Card(Color.Green, Value.Nine));

            return cards.AsEnumerable<Card>();
        }

        #endregion
    }
}
