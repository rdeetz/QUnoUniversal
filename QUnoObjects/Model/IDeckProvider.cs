// <copyright file="IDeckProvider.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides the cards that compose a <see cref="Deck"/>.
    /// </summary>
    public interface IDeckProvider
    {
        /// <summary>
        /// Provides the cards that will be added to the <see cref="Deck"/>.
        /// </summary>
        /// <returns>
        /// The cards to add to the <see cref="Deck"/>.
        /// </returns>
        IEnumerable<Card> ProvideCards();
    }
}
