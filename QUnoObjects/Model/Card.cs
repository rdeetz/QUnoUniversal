// <copyright file="Card.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Model
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Represents a card in a <see cref="Deck"/>.
    /// </summary>
    public class Card
    {
        private Color color;
        private Value value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Card"/> class.
        /// </summary>
        /// <param name="color">
        /// The color of the card.
        /// </param>
        /// <param name="value">
        /// The value of the card.
        /// </param>
        public Card(Color color, Value value)
        {
            this.color = color;
            this.value = value;
        }

        /// <summary>
        /// Gets the color of the card.
        /// </summary>
        /// <value>
        /// The color of the card.
        /// </value>
        public Color Color
        {
            get
            {
                return this.color;
            }
        }

        /// <summary>
        /// Gets the value of the card.
        /// </summary>
        /// <value>
        /// The value of the card.
        /// </value>
        public Value Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.CurrentCulture, @"{0} {1}", this.color.ToString(), this.value.ToString());
        }
    }
}
