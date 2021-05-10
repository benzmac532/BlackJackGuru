using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.GameItems;
using BlackJackGuru.Enumerations;
using BlackJackGuru.Participants;

namespace BlackJackGuru.Strategy.Calculators
{
    public static class HardCalculator
    {
        public static PlayerAction GetPlayerAction(Hand playerHand, Dealer dealer)
        {
            Card dealerCard = dealer.FaceUpCard;
            int playerHandValue = playerHand.Value;

            if (playerHandValue >= 17)
            {
                return PlayerAction.Stand;
            }
            else if (playerHandValue <= 8)
            {
                return PlayerAction.Hit;
            }
            else if (playerHandValue == 11)
            {
                return PlayerAction.Double;
            }
            else
            {
                return CalculateHardAction(playerHandValue, dealerCard.Value);
            }
        }

        #region Private Helper Methods

        private static PlayerAction CalculateHardAction(int playerHandValue, CardValue dealerValue)
        {
            PlayerAction playerAction = PlayerAction.Stand;

            if (playerHandValue >= 13 && playerHandValue <= 16)
            {
                playerAction = CalculateThirteenThroughSixteenHardValue(dealerValue);
            }
            else if (playerHandValue == 12)
            {
                playerAction = CalculateTwelveHardValue(dealerValue);
            }
            else if (playerHandValue == 10)
            {
                playerAction = CalculateTenHardValue(dealerValue);
            }
            else if (playerHandValue == 9)
            {
                playerAction = CalculateNineHardValue(dealerValue);
            }


            return playerAction;
        }

        private static PlayerAction CalculateThirteenThroughSixteenHardValue(CardValue dealerValue)
        {
            if (dealerValue >= CardValue.Seven)
            {
                return PlayerAction.Hit;
            }
            else
            {
                return PlayerAction.Stand;
            }
        }

        private static PlayerAction CalculateTwelveHardValue(CardValue dealerValue)
        {
            if (dealerValue >= CardValue.Seven || dealerValue <= CardValue.Three)
            {
                return PlayerAction.Hit;
            }
            else
            {
                return PlayerAction.Stand;
            }
        }

        private static PlayerAction CalculateTenHardValue(CardValue dealerValue)
        {
            if (dealerValue >= CardValue.Ten)
            {
                return PlayerAction.Hit;
            }
            else
            {
                return PlayerAction.Double;
            }
        }

        private static PlayerAction CalculateNineHardValue(CardValue dealerValue)
        {
            if (dealerValue >= CardValue.Seven || dealerValue == CardValue.Two)
            {
                return PlayerAction.Hit;
            }
            else
            {
                return PlayerAction.Double;
            }
        }

        #endregion

    }
}
