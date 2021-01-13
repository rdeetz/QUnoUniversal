// <copyright file="Hand.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Model
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the current <see cref="Card"/>s in a <see cref="Player"/>'s hand.
    /// </summary>
    public class Hand
    {
        private ObservableCollection<Card> cards;

        /// <summary>
        /// Initializes a new instance of the <see cref="Hand"/> class.
        /// </summary>
        public Hand()
        {
            this.cards = new ObservableCollection<Card>();
        }

        /// <summary>
        /// Gets the <see cref="Card"/>s in this <see cref="Player"/>'s hand.
        /// </summary>
        /// <value>
        /// The cards in the player's hand.
        /// </value>
        public ObservableCollection<Card> Cards
        {
            get
            {
                return this.cards;
            }
        }
    }
}
