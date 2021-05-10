using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BlackJackGuru.GameItems
{
    public class Shoe
    {
        private readonly int numberOfDecks;
        private List<Deck> decks = new List<Deck>();
        private List<Card> cards = new List<Card>();
        private static Random rng = new Random();
        private int cutMarkerIndex;
        private int cardsRemoved;

        public Shoe(int NumberOfDecks)
        {
            this.numberOfDecks = NumberOfDecks;
            this.cardsRemoved = 0;
            Reset();
        }

        #region Public Properties

        public bool PastCutMarker
        {
            get { return this.cardsRemoved >= cutMarkerIndex; }
        }

        public int CardsRemaining
        {
            get { return cards.Count(); }
        }

        public bool IsEmpty
        {
            get { return this.cards.Count == 0; }
        }

        #endregion

        #region Public Methods

        public Card GetNextCard()
        {
            if(this.cards.Count > 0)
            {
                Card card = this.cards[0];
                this.cards.RemoveAt(0);
                cardsRemoved++;
                return card;
            }
            else
            {
                return null;
            }
            
        }

        public void Reset()
        {
            this.decks.Clear();
            this.cards.Clear();

            //Initialize the number of needed decks
            for (int i = 0; i < this.numberOfDecks; i++)
            {
                this.decks.Add(new Deck());
            }

            //Shuffle the decks
            foreach (Deck deck in this.decks)
            {
                deck.ShuffleCards();
            }

            //Combine all the cards from the decks into our master list
            foreach (Deck deck in this.decks)
            {
                foreach (Card card in deck.Cards)
                {
                    this.cards.Add(card);
                }
            }

            //Shuffle all of the cards that are now combined
            ShuffleCards();

            SetCutMarkerIndex();

            cardsRemoved = 0;
        }

        public void ShuffleCards()
        {
            rng = new Random();
            this.cards = this.cards.OrderBy(x => rng.Next()).ToList();
        }

        private void SetCutMarkerIndex()
        {
            Random random = new Random();
            int halfOfCards = this.cards.Count / 2;
            cutMarkerIndex = halfOfCards + random.Next(halfOfCards / -4, halfOfCards / 4);
        }

        public override string ToString()
        {
            string cardString = string.Empty;
            int count = 1;

            foreach (Card card in this.cards)
            {
                cardString += "Card: " + count + " " + card + "\n";
                count++;
            }

            return "Number of Decks: " + this.numberOfDecks + "\n" + cardString;
        }

        #endregion

    }
}
