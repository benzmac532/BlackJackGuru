using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.Enumerations;

namespace BlackJackGuru.Strategy.Betting
{
    public abstract class BettingStrategy
    {
        protected HandResult previousResult = HandResult.Lose;

        public BettingStrategy()
        {

        }

        public int MinimumStake { get; set; }

        public int CurrentStake { get; set; }

        public abstract int StartingStake { get; set; }

        public abstract int GetAmountToBet();

        public abstract void Reset();

        public void UpdateForWin()
        {
            previousResult = HandResult.Win;
        }

        public void UpdateForLoss()
        {
            previousResult = HandResult.Lose;
        }

        public void UpdateForPush()
        {
            previousResult = HandResult.Push;
        }

    }
}
