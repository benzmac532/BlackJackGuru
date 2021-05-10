using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.Enumerations;
using BlackJackGuru.Participants;
using BlackJackGuru.GameItems;

namespace BlackJackGuru.Strategy.PlayerActions
{
    public static class IllustriousEighteen
    {
        public static PlayerAction CalculateStrategy(Dealer dealer, Hand playerHand, HiLoCounter counter, bool hasSplitHand)
        {
            int trueCount = counter.TrueCount;
            CardValue dealerValue = dealer.FaceUpCard.Value;

            if ((playerHand.Value == 16 && dealerValue == CardValue.Ten && dealerValue != CardValue.Ace && trueCount >= 0) ||
                (playerHand.Value == 15 && dealerValue >= CardValue.Ten && dealerValue != CardValue.Ace && counter.TrueCount >= 4) ||
                (playerHand.Value == 16 && dealerValue == CardValue.Nine && trueCount >= 5) ||
                (playerHand.Value == 10 && dealerValue == CardValue.Three && trueCount >= 2) ||
                (playerHand.Value == 10 && dealerValue == CardValue.Two && trueCount >= 4))
            {
                return PlayerAction.Stand;
            }
            else if ((playerHand.HasPair && playerHand.Value == 10 && dealerValue == CardValue.Five && trueCount >= 5) ||
                     (playerHand.HasPair && playerHand.Value == 10 && dealerValue == CardValue.Six && trueCount >= 4))
            {
                if (hasSplitHand)
                {
                    return PlayerAction.NoAction;
                }

                return PlayerAction.Split;
            }
            else if ((playerHand.Value == 11 && dealerValue == CardValue.Ace && trueCount >= 1) ||
                     (playerHand.Value == 9 && dealerValue == CardValue.Two && trueCount >= 1)  ||
                     (playerHand.Value == 10 && dealerValue == CardValue.Ace && trueCount >= 4) ||
                     (playerHand.Value == 9 && dealerValue == CardValue.Seven && trueCount >= 4) ||
                     (playerHand.HasPair && playerHand.FirstCard.Value >= CardValue.Ten && dealerValue >= CardValue.Ten && dealerValue != CardValue.Ace && trueCount >= 4))
            {
                return PlayerAction.Double;
            }
            else if ((playerHand.Value == 12 && dealerValue == CardValue.Four && trueCount <= 0) ||
                     (playerHand.Value == 12 && dealerValue == CardValue.Five && trueCount <= -1) ||
                     (playerHand.Value == 12 && dealerValue == CardValue.Six && trueCount <= -1) ||
                     (playerHand.Value == 14 && dealerValue == CardValue.Three && trueCount <= 0) ||
                     (playerHand.Value == 13 && dealerValue == CardValue.Three && trueCount <= 2))
            {
                return PlayerAction.Hit;
            }
            else
            {
                return PlayerAction.NoAction;

            }
        }

        public static bool NeedsInsurance(Dealer dealer, Hand playerHand, HiLoCounter counter)
        {
            if(counter.TrueCount > 3 && dealer.FaceUpCard.Value == CardValue.Ace)
            {
                return true;
            }

            return false;
        }
    }
}
