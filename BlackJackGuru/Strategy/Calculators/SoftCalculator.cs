using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.Enumerations;
using BlackJackGuru.Participants;
using BlackJackGuru.GameItems;

namespace BlackJackGuru.Strategy.Calculators
{
    public static class SoftCalculator
    {
        public static PlayerAction GetPlayerAction(Hand playerHand, Dealer dealer)
        {
            Card playerNonAceCard = (playerHand.FirstCard.Value != CardValue.Ace) ? playerHand.FirstCard : playerHand.SecondCard;
            Card dealerCard = dealer.FaceUpCard;

            if (playerNonAceCard.Value >= CardValue.Eight)
            {
                return PlayerAction.Stand;
            }
            else
            {
                return CalculateSoftAction(playerNonAceCard, dealerCard);
            }
        }

        #region Private Helper Methods

        private static PlayerAction CalculateSoftAction(Card playerCard, Card dealerCard)
        {
            PlayerAction playerAction = PlayerAction.Stand;

            if (playerCard.Value == CardValue.Seven)
            {
                playerAction = CalculateSevenSoftAction(dealerCard.Value);
            }
            else if (playerCard.Value == CardValue.Six)
            {
                playerAction = CalculateSixSoftAction(dealerCard.Value);
            }
            else if (playerCard.Value == CardValue.Four || playerCard.Value == CardValue.Five)
            {
                playerAction = CalculateFourAndFiveSoftAction(dealerCard.Value);
            }
            else if (playerCard.Value == CardValue.Two || playerCard.Value == CardValue.Three)
            {
                playerAction = CalculateTwoAndThreeSoftAction(dealerCard.Value);
            }

            return playerAction;
        }

        private static PlayerAction CalculateSevenSoftAction(CardValue dealerValue)
        {
            if (dealerValue >= CardValue.Nine)
            {
                return PlayerAction.Hit;
            }
            else
            {
                return PlayerAction.Stand;
            }
        }

        private static PlayerAction CalculateSixSoftAction(CardValue dealerValue)
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

        private static PlayerAction CalculateFourAndFiveSoftAction(CardValue dealerValue)
        {
            if (dealerValue >= CardValue.Seven || dealerValue <= CardValue.Three)
            {
                return PlayerAction.Hit;
            }
            else
            {
                return PlayerAction.Double;
            }
        }

        private static PlayerAction CalculateTwoAndThreeSoftAction(CardValue dealerValue)
        {
            if (dealerValue >= CardValue.Seven || dealerValue <= CardValue.Four)
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
