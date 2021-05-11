using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.Enumerations;

namespace BlackJackGuru.Strategy.Betting
{
    public class Manhattan : BettingStrategy
    {
        private bool performedStepDown = false;

        public Manhattan(int minStake)
        {
            MinimumStake = minStake;
            CurrentStake = 2 * MinimumStake;
        }

        public override int StartingStake { get { return MinimumStake * 2; } set { } }

        public override int GetAmountToBet()
        {          
            if(previousResult == HandResult.Lose)
            {
                performedStepDown = false;
                CurrentStake = StartingStake;
            }
            else if(previousResult == HandResult.Win)
            {
                if(CurrentStake == StartingStake && ! performedStepDown)
                {
                    CurrentStake = MinimumStake;
                    performedStepDown = true;
                }
                else
                {
                    CurrentStake += MinimumStake;
                }
            }

            return CurrentStake;
        }

        public override void Reset()
        {
            CurrentStake = 2 * MinimumStake;
        }
    }
}
