// <copyright file="UniversalGameSerializer.cs" company="Mooville">
//   Copyright © 2021 Roger Deetz. All rights reserved.
// </copyright>

namespace Mooville.QUno.Universal.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Windows.Storage;
    using Mooville.QUno.Model;

    public class UniversalGameSerializer : IUniversalGameSerializer
    {
        // If this code looks a lot like QUnoLibrary's StandardGameSerializer, 
        // that is because it is. The internals are the same but the 
        // public interface has been adapted to work with the Windows.Storage APIs.

        public UniversalGameSerializer()
        {
        }

        #region IUniversalGameSerializer Members

        public async Task<Game> LoadFromFileAsync(IStorageFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            var gameText = await FileIO.ReadTextAsync(file);

            XDocument document = XDocument.Parse(gameText);
            XElement rootElement = document.Element("quno");
            XElement gameElement = rootElement.Element("game");
            Game game = this.CreateGame(gameElement);

            return game;
        }

        public async void SaveToFileAsync(Game game, IStorageFile file)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            XDocument document = new XDocument(new XElement("quno", this.CreateGameElement(game)));
            var gameText = document.ToString();

            await FileIO.WriteTextAsync(file, gameText);

            return;
        }

        #endregion

        private Game CreateGame(XElement gameElement)
        {
            XElement deckElement = gameElement.Element("deck");
            Deck deck = this.CreateDeck(deckElement);

            XElement playersElement = gameElement.Element("players");
            IEnumerable<Player> players = this.GetPlayers(playersElement);

            Direction direction = this.GetDirection(gameElement);
            string currentPlayerName = this.GetCurrentPlayerName(gameElement);

            // The standard serializer assumes the standard rule provider.
            Game game = new Game(deck, new StandardRuleProvider());

            foreach (Player player in players)
            {
                game.Players.Add(player);
            }

            game.CurrentDirection = direction;

            int currentPlayerIndex = this.FindCurrentPlayerIndex(currentPlayerName, players);
            game.CurrentPlayerIndex = currentPlayerIndex;

            return game;
        }

        private Direction GetDirection(XElement gameElement)
        {
            string attribute = (string)gameElement.Attribute("currentDirection");
            Direction direction = (Direction)Enum.Parse(typeof(Direction), attribute);

            return direction;
        }

        private string GetCurrentPlayerName(XElement gameElement)
        {
            string currentPlayerName = (string)gameElement.Attribute("currentPlayer");

            return currentPlayerName;
        }

        private int FindCurrentPlayerIndex(string currentPlayerName, IEnumerable<Player> players)
        {
            int index = 0;

            foreach (var player in players)
            {
                if (player.Name.Equals(currentPlayerName, StringComparison.InvariantCulture))
                {
                    return index;
                }
                else
                {
                    index++;
                }
            }

            if (index > (players.Count() - 1))
            {
                // This should never happen.
                // We should probably throw and let the UI handle bad files.
                return 0;
            }

            return index;
        }

        private Deck CreateDeck(XElement deckElement)
        {
            // When deserializing, we don't want to add the default cards to the deck.
            Deck deck = new Deck(new EmptyDeckProvider());

            Color? currentWildColor = this.GetCurrentWildColor(deckElement);
            deck.CurrentWildColor = currentWildColor;

            XElement discardPileElement = deckElement.Element("discardPile");
            IEnumerable<Card> discardPile = this.GetDiscardPile(discardPileElement);
            foreach (Card card in discardPile.Reverse())
            {
                deck.DiscardPile.Push(card);
            }

            XElement drawPileElement = deckElement.Element("drawPile");
            IEnumerable<Card> drawPile = this.GetDrawPile(drawPileElement);
            foreach (Card card in drawPile.Reverse())
            {
                deck.DrawPile.Push(card);
            }

            return deck;
        }

        private Color? GetCurrentWildColor(XElement deckElement)
        {
            Color? color = null;
            string attribute = (string)deckElement.Attribute("currentWildColor");

            if (!String.IsNullOrEmpty(attribute))
            {
                color = (Color)Enum.Parse(typeof(Color), attribute);
            }

            return color;
        }

        private IEnumerable<Card> GetDiscardPile(XElement discardPileElement)
        {
            IEnumerable<Card> cards = from cardElement in discardPileElement.Elements("card") select this.CreateCard(cardElement);

            return cards;
        }

        private IEnumerable<Card> GetDrawPile(XElement drawPileElement)
        {
            IEnumerable<Card> cards = from cardElement in drawPileElement.Elements("card") select this.CreateCard(cardElement);

            return cards;
        }

        private IEnumerable<Player> GetPlayers(XElement playersElement)
        {
            IEnumerable<Player> players = from playerElement in playersElement.Elements("player") select this.CreatePlayer(playerElement);

            return players;
        }

        private Player CreatePlayer(XElement playerElement)
        {
            Player player = new Player();

            player.Name = (string)playerElement.Attribute("name");
            player.IsHuman = (bool)playerElement.Attribute("isHuman");

            XElement handElement = playerElement.Element("hand");
            IEnumerable<Card> cards = this.GetCards(handElement);
            foreach (Card card in cards)
            {
                player.Hand.Cards.Add(card);
            }

            return player;
        }

        private IEnumerable<Card> GetCards(XElement handElement)
        {
            IEnumerable<Card> cards = from cardElement in handElement.Elements("card") select this.CreateCard(cardElement);

            return cards;
        }

        private Card CreateCard(XElement cardElement)
        {
            Color color = this.GetColor(cardElement);
            Value value = this.GetValue(cardElement);
            Card card = new Card(color, value);

            return card;
        }

        private Color GetColor(XElement cardElement)
        {
            string attribute = (string)cardElement.Attribute("color");
            Color color = (Color)Enum.Parse(typeof(Color), attribute);

            return color;
        }

        private Value GetValue(XElement cardElement)
        {
            string attribute = (string)cardElement.Attribute("value");
            Value value = (Value)Enum.Parse(typeof(Value), attribute);

            return value;
        }

        private XElement CreateGameElement(Game game)
        {
            return new XElement(
                "game",
                new XAttribute("currentDirection", game.CurrentDirection),
                new XAttribute("currentPlayer", game.CurrentPlayer.Name),
                new XAttribute("date", DateTime.UtcNow),
                this.CreateDeckElement(game.Deck),
                this.CreatePlayersElement(game.Players));
        }

        private XElement CreateDeckElement(Deck deck)
        {
            return new XElement(
                "deck",
                new XAttribute("currentWildColor", deck.CurrentWildColor.HasValue ? deck.CurrentWildColor.ToString() : String.Empty),
                this.CreateDiscardPileElement(deck),
                this.CreateDrawPileElement(deck));
        }

        private XElement CreateDiscardPileElement(Deck deck)
        {
            return new XElement(
                "discardPile",
                deck.DiscardPile.Select(card => this.CreateCardElement(card)));
        }

        private XElement CreateDrawPileElement(Deck deck)
        {
            return new XElement(
                "drawPile",
                deck.DrawPile.Select(card => this.CreateCardElement(card)));
        }

        private XElement CreatePlayersElement(IEnumerable<Player> players)
        {
            return new XElement(
                "players",
                players.Select(player => this.CreatePlayerElement(player)));
        }

        private XElement CreatePlayerElement(Player player)
        {
            return new XElement(
                "player",
                new XAttribute("name", player.Name),
                new XAttribute("isHuman", player.IsHuman),
                this.CreateHandElement(player.Hand));
        }

        private XElement CreateHandElement(Hand hand)
        {
            return new XElement(
                "hand",
                hand.Cards.Select(card => this.CreateCardElement(card)));
        }

        private XElement CreateCardElement(Card card)
        {
            return new XElement(
                "card",
                new XAttribute("color", card.Color),
                new XAttribute("value", card.Value));
        }
    }
}
