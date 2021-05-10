using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.Enumerations;

namespace BlackJackGuru.GameItems
{
    public class Hand
    {
        private readonly int AceValue = 11;

        public Hand()
        {
            this.FirstCard = null;
            this.SecondCard = null;
            this.Cards = new List<Card>();
        }

        #region Public Properties

        public Card FirstCard { get; private set; }

        public Card SecondCard { get; private set; }

        public List<Card> Cards { get; }

        public bool HasPair
        {
            get { return (this.FirstCard.Value == this.SecondCard.Value); }
        }

        public bool HasBlackjack
        {
            get { return (this.FirstCard.IntValue + this.SecondCard.IntValue == 21); }
        }

        public bool Busted
        {
            get { return TotalInHand() > 21; }
        }

        public bool AceInHand
        {
            get
            {
                bool firstCardIsAce = this.FirstCard.Value == CardValue.Ace;
                bool secondCardIsAce = this.SecondCard.Value == CardValue.Ace;
                return (firstCardIsAce || secondCardIsAce);
            }
        }

        public int Value
        {
            get { return TotalInHand(); }
        }

        #endregion

        #region Public Methods

        public void AddCard(Card card)
        {
            if(FirstCard == null)
            {
                AddFirstCard(card);
            }
            else if(SecondCard == null)
            {
                AddSecondCard(card);
            }

            Cards.Add(card);
        }

        public Card Split()
        {
            Card card = this.SecondCard;
            this.Cards.Remove(card);
            this.SecondCard = null;
            return card;
        }

        public void ResetHand()
        {
            this.FirstCard = null;
            this.SecondCard = null;
            this.Cards.Clear();
        }

        public override string ToString()
        {
            return "Card 1: " + this.FirstCard + ", Card 2: " + this.SecondCard;
        }

        #endregion

        #region Private Methods

        private void AddFirstCard(Card firstCard)
        {
            this.FirstCard = firstCard;
        }

        private void AddSecondCard(Card secondCard)
        {
            this.SecondCard = secondCard;
        }

        private int TotalInHand()
        {
            int total = 0;
            bool containsAce = false;

            foreach(Card card in this.Cards)
            {
                total += card.IntValue;

                if(card.Value == CardValue.Ace)
                {
                    containsAce = true;
                }
            }

            //check to see if we need to use a '1' value for any aces instead of 11
            if(total > 21 && containsAce)
            {
                total = 0;

                foreach(Card card in this.Cards)
                {
                    if((card.Value == CardValue.Ace) && (total + AceValue > 21))
                    {
                        total += 1;
                    }
                    else
                    {
                        total += card.IntValue;
                    }
                }
            }

            return total;
        }

        #endregion

    }
}
