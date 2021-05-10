using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.GameItems;
using BlackJackGuru.Statistics;

namespace BlackJackGuru.Participants
{
    public class Player
    {
        public Player()
        {
            this.PrimaryHand = new Hand();
            this.SplitHand = new Hand();
            this.Statistics = new Stats();
        }

        #region Public Properties

        public Hand PrimaryHand { get; }

        public Hand SplitHand { get; }

        public Stats Statistics { get; }

        public bool HasSplitHand { get; private set; }

        #endregion

        #region Public Methods

        public void SplitPrimaryHand()
        {
            this.SplitHand.AddCard(this.PrimaryHand.Split());
            HasSplitHand = true;
        }

        public void AddCardToMainHand(Card card)
        {
            this.PrimaryHand.AddCard(card);
        }

        public void ResetHands()
        {
            this.PrimaryHand.ResetHand();
            this.SplitHand.ResetHand();
            HasSplitHand = false;
        }

        public void ResetStatistics()
        {
            this.Statistics.Reset();
        }

        #endregion

    }
}
