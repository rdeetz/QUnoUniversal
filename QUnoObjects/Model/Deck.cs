// <copyright file="Deck.cs" company="Mooville">
//   Copyright © 2018 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the deck of <see cref="Card"/>s.
    /// </summary>
    public class Deck
    {
        private Color? currentWildColor;
        private Stack<Card> drawPile;
        private Stack<Card> discardPile;

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class.
        /// </summary>
        public Deck()
            : this(new StandardDeckProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class.
        /// </summary>
        /// <param name="deckProvider">
        /// An implementation of <see cref="IDeckProvider"/> that creates the cards for the deck.
        /// </param>
        public Deck(IDeckProvider deckProvider)
        {
            if (deckProvider == null)
            {
                throw new ArgumentNullException("deckProvider");
            }

            this.drawPile = new Stack<Card>();
            this.discardPile = new Stack<Card>();

            foreach (var card in deckProvider.ProvideCards())
            {
                this.drawPile.Push(card);
            }
        }

        /// <summary>
        /// Occurs when the discard pile needs to be reshuffled so that a new card can be drawn.
        /// </summary>
        public event EventHandler Reshuffle;

        /// <summary>
        /// Gets the <see cref="Card"/> on the top of the <see cref="P:DiscardPile"/>.
        /// </summary>
        /// <value>
        /// The <see cref="Card"/> on the top of the <see cref="P:DiscardPile"/>.
        /// </value>
        public Card CurrentCard
        {
            get
            {
                Card card = null;

                if (this.discardPile.Count > 0)
                {
                    card = this.discardPile.Peek();
                }

                return card;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Color"/> specified for last wild card played.
        /// </summary>
        /// <value>
        /// The <see cref="Color"/> specified for last wild card played.
        /// </value>
        public Color? CurrentWildColor
        {
            get
            {
                return this.currentWildColor;
            }

            set
            {
                if (value != null)
                {
                    if (value == Color.Wild)
                    {
                        throw new ArgumentOutOfRangeException("value", "Wild is not a valid value for CurrentWildColor.");
                    }
                }

                this.currentWildColor = value;
            }
        }

        /// <summary>
        /// Gets the draw pile.
        /// </summary>
        /// <value>
        /// The draw pile.
        /// </value>
        public Stack<Card> DrawPile
        {
            get
            {
                return this.drawPile;
            }
        }

        /// <summary>
        /// Gets the discard pile.
        /// </summary>
        /// <value>
        /// The discard pile.
        /// </value>
        public Stack<Card> DiscardPile
        {
            get
            {
                return this.discardPile;
            }
        }

        /// <summary>
        /// Shuffles the cards in the <see cref="Deck"/>.
        /// </summary>
        /// <remarks>
        /// This method is intended to be called before a game starts, 
        /// when all cards are on the draw pile.
        /// </remarks>
        public void Shuffle()
        {
            Card[] cardsToShuffle = Deck.GetCardsOffPile(ref this.drawPile);
            Deck.ShuffleCards(ref cardsToShuffle);
            Deck.PutCardsOnPile(cardsToShuffle, ref this.drawPile);

            return;
        }

        /// <summary>
        /// Takes a <see cref="Card"/> off the <see cref="P:DrawPile"/> and returns it to the <see cref="Player"/>.
        /// </summary>
        /// <returns>
        /// The top <see cref="Card"/> from the <see cref="P:DrawPile"/>.
        /// </returns>
        public Card Draw()
        {
            Card card = null;

            if (this.drawPile.Count > 0)
            {
                card = this.drawPile.Pop();
            }
            else
            {
                // Save the current card, shuffle the rest from the discard pile, 
                // put those back on the draw pile, reset the current card, 
                // then pop a card off the draw pile. If there is nothing on the 
                // discard pile, don't bother shuffling; if there is nothing
                // on the draw pile, just return null.
                if (this.discardPile.Count > 0)
                {
                    Card current = this.discardPile.Pop();

                    Card[] cardsToShuffle = Deck.GetCardsOffPile(ref this.discardPile);
                    Deck.ShuffleCards(ref cardsToShuffle);
                    Deck.PutCardsOnPile(cardsToShuffle, ref this.drawPile);

                    this.discardPile.Push(current);

                    // Fire the event that the deck has been reshuffled.
                    EventHandler tmp = this.Reshuffle;

                    if (tmp != null)
                    {
                        tmp(this, new EventArgs());
                    }
                }

                if (this.drawPile.Count > 0)
                {
                    card = this.drawPile.Pop();
                }
            }

            return card;
        }

        /// <summary>
        /// Places a <see cref="Card"/> on the <see cref="P:DiscardPile"/>.
        /// </summary>
        /// <param name="card">
        /// The <see cref="Card"/> to be played.
        /// </param>
        public void Play(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException("card");
            }

            this.discardPile.Push(card);

            return;
        }

        /// <summary>
        /// Removes all cards from a pile and returns them in an array.
        /// </summary>
        /// <param name="pile">
        /// The pile of cards to empty.
        /// </param>
        /// <returns>
        /// An array of the cards that were on the pile.
        /// </returns>
        public static Card[] GetCardsOffPile(ref Stack<Card> pile)
        {
            int count = pile.Count;
            Card[] cards = new Card[count];

            for (int i = 0; i < count; i++)
            {
                cards[i] = pile.Pop();
            }

            return cards;
        }

        /// <summary>
        /// Takes all cards from an array and puts them on a pile.
        /// </summary>
        /// <param name="cards">
        /// The cards to put on the pile.
        /// </param>
        /// <param name="pile">
        /// The pile where the cards will go.
        /// </param>
        public static void PutCardsOnPile(Card[] cards, ref Stack<Card> pile)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                pile.Push(cards[i]);
            }

            return;
        }

        /// <summary>
        /// Shuffles an array of cards.
        /// </summary>
        /// <param name="cardsToShuffle">
        /// The cards to shuffle.
        /// </param>
        public static void ShuffleCards(ref Card[] cardsToShuffle)
        {
            Random rnd = new Random();

            for (int i = cardsToShuffle.Length - 1; i > 0; i--)
            {
                int index = rnd.Next(i + 1);
                Card temp = cardsToShuffle[i];
                cardsToShuffle[i] = cardsToShuffle[index];
                cardsToShuffle[index] = temp;
            }

            return;
        }
    }
}
