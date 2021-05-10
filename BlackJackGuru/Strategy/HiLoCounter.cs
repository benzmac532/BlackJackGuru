using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.GameItems;

namespace BlackJackGuru.Strategy
{
    public class HiLoCounter
    {
        private int numberOfDecksInShoe;
        private int decksRemaining;
        private List<Card> cardsSeen;
        private const int numberOfCardsInDeck = 52;

        public HiLoCounter(int NumberOfDecksInShoe)
        {
            this.numberOfDecksInShoe = NumberOfDecksInShoe;
            this.decksRemaining = NumberOfDecksInShoe;
            this.cardsSeen = new List<Card>();
        }

        #region Public Properties & Methods

        public int RunningCount { get; private set; }

        public int TrueCount
        {
            get { return Convert.ToInt32(Math.Floor((double)RunningCount / decksRemaining)); }
        }

        public void CountCard(Card card)
        {
            this.cardsSeen.Add(card);

            //Count the card
            if(card.IntValue < 7)
            {
                RunningCount++;
            }
            else if(card.IntValue > 9)
            {
                RunningCount--;
            }

            //Update the decks remaining if needed
            if(this.cardsSeen.Count % numberOfCardsInDeck == 0)
            {
                decksRemaining--;
            }
        }

        public void Reset()
        {
            this.cardsSeen = new List<Card>();
            RunningCount = 0;
            decksRemaining = numberOfDecksInShoe;
        }

        public override string ToString()
        {
            return "Running Count: " + RunningCount + 
                   "\nTrue Count: " + TrueCount + 
                   "\nDecks Remaining: " + decksRemaining;
        }

        #endregion
    }
}
