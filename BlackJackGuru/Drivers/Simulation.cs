using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.Strategy;
using BlackJackGuru.Strategy.PlayerActions;
using BlackJackGuru.Participants;
using BlackJackGuru.Enumerations;
using BlackJackGuru.GameItems;

namespace BlackJackGuru.Drivers
{
    public class GameSimulation
    {
        private int numberOfPlayers;
        private int numberOfIterations;
        private int completedIterations;
        private int numberOfDecksInShoe;
        private Shoe shoe;
        private Dealer dealer;
        private List<Player> playerList;
        private HiLoCounter counter;

        private const string quit = "q";
        private const string restart = "r";
        private string userAction = string.Empty;

        public GameSimulation()
        {
            completedIterations = 0;
            playerList = new List<Player>();
            dealer = new Dealer();
        }

        #region Public Methods

        public void Start()
        {
            Console.WriteLine("\n\t-----------------------------------------");
            Console.WriteLine("\t----- BlackJackGuru Simulation Mode -----");
            Console.WriteLine("\t-----------------------------------------\n");

            InitializeSimulationWithUserInput();

            Console.WriteLine("\nBlackJackGuru Simulation Mode: Simulating a poker game for " + numberOfPlayers + " over " + numberOfIterations + " iterations." +
                              "\nResults will be printed after the simulation is completed.");

            while(userAction != "q")
            {
                completedIterations = 0;
                ResetAll();

                while (completedIterations != numberOfIterations)
                {
                    DealCards();

                    if (!dealer.HasBlackjack)
                    {
                        HandlePlayerAction();

                        HandleDealerAction();
                    }

                    ComputeResults();

                    ResetParticipantHands();

                    completedIterations++;
                    Console.WriteLine("Completed " + completedIterations + "/" + numberOfIterations + " iterations.");
                }

                PrintStats();

                Console.WriteLine("\nPress \"q\" to quit or \"r\" to restart.");
                userAction = Console.ReadLine();

                while (userAction != quit && userAction != restart)
                {
                    Console.WriteLine("\nInput \"" + userAction + "\" is not recognized. Please press \"q\" to quit or \"r\" to restart the simulation.");
                    userAction = Console.ReadLine();
                }
            }
        }

        #endregion

        #region Private Helper Methods

        private void PrintStats()
        {
            for(int i=0; i<numberOfPlayers; i++)
            {
                Player p = playerList[i];
                Console.WriteLine("\nPlayer " + (i + 1) + " statistics:\n" + p.Statistics);
            }
        }

        private void ResetAll()
        {
            shoe.Reset();
            dealer.ResetHands();
            counter.Reset();
            foreach(Player player in playerList)
            {
                player.ResetHands();
                player.ResetStatistics();
            }
        }

        private void ResetParticipantHands()
        {
            dealer.ResetHands();

            foreach(Player player in playerList)
            {
                player.ResetHands();
            }
        }

        private void ComputeResults()
        {
            foreach(Player player in playerList)
            {
                ComputeResults(player, player.PrimaryHand);

                if (player.HasSplitHand)
                {
                    ComputeResults(player, player.SplitHand);
                }
            }
        }

        private void ComputeResults(Player player, Hand playerHand)
        {
            if (playerHand.Busted)
            {
                player.Statistics.RecordLoss();                
            }
            else if(playerHand.HasBlackjack && dealer.HasBlackjack)
            {
                player.Statistics.RecordPush();
            }
            else if (dealer.Busted)
            {
                player.Statistics.RecordWin();
            }
            else
            {
                int playerValue = playerHand.Value;
                int dealerValue = dealer.Value;

                if (dealerValue == playerValue)
                {
                    player.Statistics.RecordPush();
                }
                else if(playerValue > dealerValue)
                {
                    player.Statistics.RecordWin();
                }
                else
                {
                    player.Statistics.RecordLoss();
                }
            }
        }

        private void HandleDealerAction()
        {
            int value;
            bool actionFinished = false;

            while (!actionFinished)
            {
                value = dealer.Value;

                if (value <= 16)
                {
                    Hit(dealer.PrimaryHand);
                }
                
                if (value >= 17 || dealer.Busted)
                {
                    actionFinished = true;
                }
            }
        }

        private void HandlePlayerAction()
        {
            foreach (Player player in playerList)
            {
                HandleMainHand(player);

                HandleSplitHand(player);
            }
        }

        private void HandleMainHand(Player player)
        {
            bool actionFinished = false;

            while (!actionFinished)
            {
                PlayerAction basicAction = BasicStrategy.CalculateStrategy(dealer, player.PrimaryHand, player.HasSplitHand);
                PlayerAction illustriousAction = IllustriousEighteen.CalculateStrategy(dealer, player.PrimaryHand, counter, player.HasSplitHand);

                switch (illustriousAction)
                {
                    case PlayerAction.Stand:
                        actionFinished = true;
                        break;
                    case PlayerAction.Hit:
                        Hit(player.PrimaryHand);
                        break;
                    case PlayerAction.Double:
                        Hit(player.PrimaryHand);
                        actionFinished = true;
                        break;
                    case PlayerAction.Split:
                        if (!player.HasSplitHand)
                        {
                            player.SplitPrimaryHand();
                            Hit(player.PrimaryHand);
                            Hit(player.SplitHand);
                            player.Statistics.RecordSplitHand();
                        }
                        break;
                    case PlayerAction.NoAction:
                        switch (basicAction)
                        {
                            case PlayerAction.Stand:
                                actionFinished = true;
                                break;
                            case PlayerAction.Hit:
                                Hit(player.PrimaryHand);
                                break;
                            case PlayerAction.Double:
                                Hit(player.PrimaryHand);
                                actionFinished = true;
                                break;
                            case PlayerAction.Split:
                                if (!player.HasSplitHand)
                                {
                                    player.SplitPrimaryHand();
                                    Hit(player.PrimaryHand);
                                    Hit(player.SplitHand);
                                    player.Statistics.RecordSplitHand();
                                }
                                break;
                        }
                        break;
                }
            }
        }

        private void HandleSplitHand(Player player)
        {
            bool actionFinished = false;

            if (player.HasSplitHand)
            {
                while (!actionFinished)
                {
                    PlayerAction basicAction = BasicStrategy.CalculateStrategy(dealer, player.SplitHand, player.HasSplitHand);
                    PlayerAction illustriousAction = IllustriousEighteen.CalculateStrategy(dealer, player.SplitHand, counter, player.HasSplitHand);

                    switch (illustriousAction)
                    {
                        case PlayerAction.Stand:
                            actionFinished = true;
                            break;
                        case PlayerAction.Hit:
                            Hit(player.SplitHand);
                            break;
                        case PlayerAction.Double:
                            Hit(player.SplitHand);
                            actionFinished = true;
                            break;
                        case PlayerAction.Split:
                            actionFinished = true;
                            break;
                        case PlayerAction.NoAction:
                            switch (basicAction)
                            {
                                case PlayerAction.Stand:
                                    actionFinished = true;
                                    break;
                                case PlayerAction.Hit:
                                    Hit(player.SplitHand);
                                    break;
                                case PlayerAction.Double:
                                    Hit(player.SplitHand);
                                    actionFinished = true;
                                    break;
                                case PlayerAction.Split:
                                    actionFinished = true;
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        private void Hit(Hand hand)
        {
            Card card = shoe.GetNextCard();

            hand.AddCard(card);
            counter.CountCard(card);
        }
        
        private void DealCards()
        {
            PerformShoeMaintenance();
            DealOneCardToAll();
            DealOneCardToAll();
        }

        private void PerformShoeMaintenance()
        {
            if (shoe.PastCutMarker)
            {
                shoe.Reset();
                counter.Reset();
            }
        }

        private void DealOneCardToAll()
        {
            Card card = null;

            foreach(Player player in playerList)
            {
                card = shoe.GetNextCard();
                player.AddCardToMainHand(card);
                counter.CountCard(card);
            }

            card = shoe.GetNextCard();
            dealer.AddCardToMainHand(card);
            counter.CountCard(card);
        }
        
        private void InitializeSimulationWithUserInput()
        {
            string input = string.Empty;

            Console.WriteLine("\nPlease enter the number of players.");
            input = Console.ReadLine();

            while (!int.TryParse(input, out numberOfPlayers))
            {
                Console.WriteLine("\nInput: \"" + input + "\" is not a valid integer. Please enter a valid integer for the number of players.");
                input = Console.ReadLine();
            }

            Console.WriteLine("\nPlease enter the number of total iterations to simulate (one iteration is a full played hand per player)");
            input = Console.ReadLine();

            while (!int.TryParse(input, out numberOfIterations))
            {
                Console.WriteLine("\nInput: \"" + input + "\" is not a valid integer. Please enter a valid integer for the number of iterations.");
                input = Console.ReadLine();
            }

            Console.WriteLine("\nPlease enter the number of decks in the shoe.");
            input = Console.ReadLine();

            while (!int.TryParse(input, out numberOfDecksInShoe))
            {
                Console.WriteLine("\nInput: \"" + input + "\" is not a valid integer. Please enter a valid integer for the number of decks in the shoe.");
                input = Console.ReadLine();
            }

            shoe = new Shoe(numberOfDecksInShoe);
            counter = new HiLoCounter(numberOfDecksInShoe);

            for (int i=0; i<numberOfPlayers; i++)
            {
                playerList.Add(new Player());
            }
        }

        #endregion

    }
}
