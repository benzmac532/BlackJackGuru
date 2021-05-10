using System;
using System.Collections.Generic;
using System.Linq;
using BlackJackGuru.Enumerations;

namespace BlackJackGuru.GameItems
{
    public class Deck
    {
        public Deck()
        {
            Cards = new List<Card>();
            InitializeCardsInDeck();
        }

        #region Public Properties

        public List<Card> Cards { get; private set; }

        #endregion

        #region Public Methods

        public void ShuffleCards()
        {
            Random rng = new Random();
            Cards = Cards.OrderBy(x => rng.Next()).ToList();
        }

        public void RemoveCardFromDeck(Card card)
        {
            if (Cards.Contains(card))
            {
                Cards.Remove(card);
            }
        }

        private void InitializeCardsInDeck()
        {
            foreach(Suits suit in Enum.GetValues(typeof(Suits)))
            {
                foreach(CardValue cardValue in Enum.GetValues(typeof(CardValue)))
                {
                    Card card = new Card(suit, cardValue);
                    Cards.Add(card);
                }
            }
        }

        public override string ToString()
        {
            string deckString = string.Empty;
            int count = 1;

            foreach(Card card in Cards)
            {
                deckString += "Card: " + count + " " + card + "\n";
                count++;
            }

            return deckString;
        }

        #endregion

    }
}
