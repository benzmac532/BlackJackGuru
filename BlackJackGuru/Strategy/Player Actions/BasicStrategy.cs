using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.Participants;
using BlackJackGuru.Enumerations;
using BlackJackGuru.Strategy.Calculators;
using BlackJackGuru.GameItems;

namespace BlackJackGuru.Strategy.PlayerActions
{
    public static class BasicStrategy
    {
        public static PlayerAction CalculateStrategy(Dealer dealer, Hand playerHand, bool alreadySplit)
        {
            PlayerAction playerAction = PlayerAction.Stand;

            if (playerHand.HasBlackjack || playerHand.Busted)
            {
                return PlayerAction.Stand;
            }
            else if (playerHand.HasPair && ! alreadySplit)
            {
                playerAction = PairCalculator.GetPlayerAction(playerHand, dealer);

                if(playerAction == PlayerAction.NoSplit)
                {
                    //We need to still calculate the cards as if they are normal in the case of no split
                    playerAction = HardCalculator.GetPlayerAction(playerHand, dealer);
                }
            }
            else if (playerHand.AceInHand)
            {
                playerAction = SoftCalculator.GetPlayerAction(playerHand, dealer);
            }
            else
            {
                playerAction = HardCalculator.GetPlayerAction(playerHand, dealer);
            }

            return playerAction;
        }
    }
}
