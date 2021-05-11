using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.GameItems;
using BlackJackGuru.Statistics;
using BlackJackGuru.Strategy.Betting;

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

        public Player(Type bettingStrategy, int StartingFunds, int MinimumBetAmount)
        {
            this.PrimaryHand = new Hand();
            this.SplitHand = new Hand();
            this.Statistics = new Stats();
            this.Funds = StartingFunds;
            this.StartingFunds = StartingFunds;

            if(bettingStrategy == typeof(Manhattan))
            {
                BettingStrategy = new Manhattan(MinimumBetAmount);
            }
            else if(bettingStrategy == typeof(Martingale))
            {
                BettingStrategy = new Martingale(MinimumBetAmount);
            }
        }

        #region Public Properties

        public double StartingFunds { get; private set; }

        public double Funds { get; private set; }

        public int CurrentBet { get; private set; }

        public Hand PrimaryHand { get; }

        public Hand SplitHand { get; }

        public Stats Statistics { get; }

        public bool HasSplitHand { get; private set; }

        public BettingStrategy BettingStrategy { get; private set; } 

        public bool OutOfMoney { get { return Funds <= 0; } }

        #endregion

        #region Public Methods

        public void Bet()
        {
            Funds -= GetAmountToBet();
        }

        public void RecordWin(bool hasBlackjack)
        {
            if (hasBlackjack)
            {                
                Funds += CurrentBet + CurrentBet * 1.5;
            }
            else
            {
                Funds += CurrentBet * 2;
            }

            BettingStrategy.UpdateForWin();
            Statistics.RecordWin();
        }

        public void RecordLoss()
        {
            BettingStrategy.UpdateForLoss();
            Statistics.RecordLoss();
        }

        public void RecordPush()
        {
            BettingStrategy.UpdateForPush();
            Statistics.RecordPush();
        }

        public void SplitPrimaryHand()
        {
            this.SplitHand.AddCard(this.PrimaryHand.Split());
            HasSplitHand = true;
        }

        public void AddCardToMainHand(Card card)
        {
            this.PrimaryHand.AddCard(card);
        }

        public void ResetAll()
        {
            ResetHands();
            ResetStatistics();
            ResetFunds();
            ResetBettingStrategy();
        }

        public void ResetBettingStrategy()
        {
            BettingStrategy.Reset();
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

        public void ResetFunds()
        {
            Funds = StartingFunds;
        }

        #endregion

        private int GetAmountToBet()
        {
            int newBet = BettingStrategy.GetAmountToBet();
            CurrentBet = newBet;
            return CurrentBet;
        }

    }
}
