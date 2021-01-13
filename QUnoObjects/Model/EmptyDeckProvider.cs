// <copyright file="EmptyDeckProvider.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Model
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents an implementation of <see cref="IDeckProvider"/> that provides no cards.
    /// </summary>
    public class EmptyDeckProvider : IDeckProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyDeckProvider"/> class.
        /// </summary>
        public EmptyDeckProvider()
        {
        }

        #region IDeckProviderMembers

        /// <summary>
        /// Provides the cards that will be added to the <see cref="Deck"/>.
        /// </summary>
        /// <returns>
        /// The cards to add to the <see cref="Deck"/>.
        /// </returns>
        public IEnumerable<Card> ProvideCards()
        {
            return new List<Card>().AsEnumerable<Card>();
        }

        #endregion
    }
}
