using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.Enumerations;

namespace BlackJackGuru.Statistics
{
    public class Stats
    {
        private int currentWinStreak;
        private int currentLoseStreak;

        public Stats()
        {

        }

        #region Public Properties

        public int NumberOfWins { get; private set; }

        public int NumberOfLosses { get; private set; }

        public int NumberOfPushes { get; private set; }

        public int NumberOfHandsPlayed { get; private set; }

        public int NumberOfSplitHands { get; private set; }

        public int LongestWinStreak { get; private set; }

        public int LongestLoseStreak { get; private set; }

        public HandResult PreviousHandResult { get; private set; }

        #endregion

        #region Public Methods

        public double WinPercentage
        {
            get { return Math.Round(((double)NumberOfWins / NumberOfHandsPlayed) * 100, 2); }
        }

        public double SplitPercentage
        {
            get { return Math.Round(((double)NumberOfSplitHands / NumberOfHandsPlayed) * 100, 2); }
        }

        public double LossPercentage
        {
            get { return Math.Round(((double)NumberOfLosses / NumberOfHandsPlayed) * 100, 2); }
        }

        public double PushPercentage
        {
            get { return Math.Round(((double)NumberOfPushes / NumberOfHandsPlayed) * 100, 2); }
        }

        public void RecordWin()
        {
            NumberOfWins++;
            NumberOfHandsPlayed++;
            currentLoseStreak = 0;

            if (PreviousHandResult != HandResult.Win)
            {
                PreviousHandResult = HandResult.Win;
                currentWinStreak = 1;
            }
            else
            {
                currentWinStreak++;
            }

            if (currentWinStreak > LongestWinStreak)
            {
                LongestWinStreak = currentWinStreak;
            }
        }

        public void RecordSplitHand()
        {
            NumberOfSplitHands++;
            NumberOfHandsPlayed++;
        }

        public void RecordLoss()
        {
            NumberOfLosses++;
            NumberOfHandsPlayed++;
            currentWinStreak = 0;

            if (PreviousHandResult != HandResult.Lose)
            {
                PreviousHandResult = HandResult.Lose;
                currentLoseStreak = 1;
            }
            else
            {
                currentLoseStreak++;
            }

            if (currentLoseStreak > LongestLoseStreak)
            {
                LongestLoseStreak = currentLoseStreak;
            }
        }

        public void RecordPush()
        {
            NumberOfPushes++;
            NumberOfHandsPlayed++;
            PreviousHandResult = HandResult.Push;
        }

        public void Reset()
        {
            NumberOfWins = 0;
            NumberOfLosses = 0;
            NumberOfHandsPlayed = 0;
            NumberOfPushes = 0;
            NumberOfSplitHands = 0;
            LongestWinStreak = 0;
            LongestLoseStreak = 0;
            currentLoseStreak = 0;
            currentWinStreak = 0;
        }

        public override string ToString()
        {
            return "Wins: " + NumberOfWins + " (" + WinPercentage + "%)" +
                   "\nLosses: " + NumberOfLosses + " (" + LossPercentage + "%)" +
                   "\nPushes: " + NumberOfPushes + " (" + PushPercentage + "%)" +
                   "\nPush & Win Percent: " + (PushPercentage + WinPercentage) + "%" +
                   "\nSplit Hands: " + NumberOfSplitHands + "(" + SplitPercentage + "%)" +
                   "\nTotal Hands Played: " + NumberOfHandsPlayed +
                   "\nLongest Win Streak: " + LongestWinStreak +
                   "\nLongest Lose Streak: " + LongestLoseStreak;
        }

        #endregion
    }
}
