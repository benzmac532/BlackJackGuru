using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.Enumerations;

namespace BlackJackGuru.Strategy.Betting
{
    public class Martingale : BettingStrategy
    {
        public Martingale(int StartingBet)
        {
            StartingStake = StartingBet;
            CurrentStake = StartingBet;
        }

        public override int StartingStake { get; set; }

        public override int GetAmountToBet()
        {
            if(previousResult == HandResult.Lose)
            {
                CurrentStake *= 2;
            }
            else if(previousResult == HandResult.Win)
            {
                CurrentStake = StartingStake;
            }

            return CurrentStake;
        }

        public override void Reset()
        {
            CurrentStake = StartingStake;
        }
    }
}
