using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.Enumerations;
using BlackJackGuru.Participants;
using BlackJackGuru.GameItems;

namespace BlackJackGuru.Strategy.Calculators
{
    public static class PairCalculator
    {
        public static PlayerAction GetPlayerAction(Hand playerHand, Dealer dealer)
        {
            Card playerCard = playerHand.FirstCard;
            Card dealerCard = dealer.FaceUpCard;

            if (playerCard.Value == CardValue.Ace || playerCard.Value == CardValue.Eight)
            {
                //Always split if we have two aces or eights
                return PlayerAction.Split;
            }
            else if (playerCard.Value == CardValue.Ten || playerCard.Value == CardValue.Five)
            {
                //Never split on a 10 or 5
                return PlayerAction.NoSplit;
            }
            else
            {
                return CalculatePairAction(playerCard.Value, dealerCard.Value);
            }
        }

        #region Private Helper Methods

        private static PlayerAction CalculatePairAction(CardValue playerValue, CardValue dealerValue)
        {
            //Default to NoSplit action
            PlayerAction playerAction = PlayerAction.NoSplit;

            if (playerValue == CardValue.Nine)
            {
                playerAction = CalculateNinePairAction(dealerValue);
            }
            else if (playerValue == CardValue.Seven)
            {
                playerAction = CalculateSevenPairAction(dealerValue);
            }
            else if (playerValue == CardValue.Six)
            {
                playerAction = CalculateSixPairAction(dealerValue);
            }
            else if (playerValue == CardValue.Four)
            {
                playerAction = CalculateFourPairAction(dealerValue);
            }
            else if (playerValue == CardValue.Three || playerValue == CardValue.Two)
            {
                playerAction = CalculateTwoAndThreePairAction(dealerValue);
            }

            return playerAction;
        }

        private static PlayerAction CalculateNinePairAction(CardValue dealerValue)
        {
            if (dealerValue == CardValue.Seven || dealerValue >= CardValue.Ten)
            {
                return PlayerAction.NoSplit;
            }
            else
            {
                return PlayerAction.Split;
            }
        }

        private static PlayerAction CalculateSevenPairAction(CardValue dealerValue)
        {
            if (dealerValue >= CardValue.Eight)
            {
                return PlayerAction.NoSplit;
            }
            else
            {
                return PlayerAction.Split;
            }
        }

        private static PlayerAction CalculateSixPairAction(CardValue dealerValue)
        {
            if (dealerValue >= CardValue.Seven || dealerValue == CardValue.Two)
            {
                return PlayerAction.NoSplit;
            }
            else
            {
                return PlayerAction.Split;
            }
        }

        private static PlayerAction CalculateFourPairAction(CardValue dealerValue)
        {
            //TODO - implement the Double After Split Option
            return PlayerAction.NoSplit;
        }

        private static PlayerAction CalculateTwoAndThreePairAction(CardValue dealerValue)
        {
            if (dealerValue <= CardValue.Three || dealerValue >= CardValue.Eight)
            {
                return PlayerAction.NoSplit;
            }
            else
            {
                return PlayerAction.Split;
            }
        }

        #endregion

    }
}
