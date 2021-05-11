using System;
using System.Collections.Generic;
using System.Text;
using BlackJackGuru.Strategy;
using BlackJackGuru.Strategy.PlayerActions;
using BlackJackGuru.Participants;
using BlackJackGuru.Enumerations;
using BlackJackGuru.GameItems;
using BlackJackGuru.Strategy.Betting;

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
        private List<Player> removedPlayers;
        private HiLoCounter counter;

        private const string quit = "q";
        private const string restart = "r";
        private string userAction = string.Empty;

        public GameSimulation()
        {
            completedIterations = 0;
            playerList = new List<Player>();
            removedPlayers = new List<Player>();
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

                while (completedIterations != numberOfIterations && PlayersRemain())
                {
                    PlaceBets();

                    DealCards();

                    //TODO - Implement Insurance possibility (After betting strategies)
                    if (!dealer.HasBlackjack)
                    {
                        HandlePlayerAction();

                        HandleDealerAction();
                    }

                    ComputeResults();

                    RemovePlayersWithNoFunds();

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

        private void RemovePlayersWithNoFunds()
        {
            List<Player> playerToRemove = new List<Player>();

            foreach(Player player in playerList)
            {
                if (player.OutOfMoney)
                {
                    playerToRemove.Add(player);
                }
            }

            foreach(Player player in playerToRemove)
            {
                playerList.Remove(player);
                removedPlayers.Add(player);
            }
        }

        private bool PlayersRemain()
        {
            return playerList.Count > 0;
        }

        private void PlaceBets()
        {
            foreach(Player player in playerList)
            {
                player.Bet();
            }
        }

        private void PrintStats()
        {
            int count = 1;

            foreach(Player p in playerList)
            {
                Console.WriteLine("\n\tPlayer " + count + " statistics:\n\n" + p.Statistics);
                Console.WriteLine("Starting Funds: " + p.StartingFunds + "\nEnding Funds: " + p.Funds);
                count++;
            }

            foreach (Player p in removedPlayers)
            {
                Console.WriteLine("\n\tPlayer " + count + " statistics: LOST ALL FUNDS\n\n" + p.Statistics);
                Console.WriteLine("Starting Funds: " + p.StartingFunds + "\nEnding Funds: " + p.Funds);
                count++;
            }
        }

        private void ResetAll()
        {
            shoe.Reset();
            dealer.ResetHands();
            counter.Reset();

            foreach(Player player in removedPlayers)
            {
                playerList.Add(player);
            }

            removedPlayers.Clear();

            foreach(Player player in playerList)
            {
                player.ResetAll();
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
                player.RecordLoss();                
            }
            else if(playerHand.HasBlackjack && dealer.HasBlackjack)
            {
                player.RecordPush();
            }
            else if (dealer.Busted)
            {
                player.RecordWin(playerHand.HasBlackjack);
            }
            else
            {
                int playerValue = playerHand.Value;
                int dealerValue = dealer.Value;

                if (dealerValue == playerValue)
                {
                    player.RecordPush();
                }
                else if(playerValue > dealerValue)
                {
                    player.RecordWin(playerHand.HasBlackjack);
                }
                else
                {
                    player.RecordLoss();
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
            int bettingStrategy;
            int startingFunds;
            int minimumBet;

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

            Console.WriteLine("\nPlease enter the starting funds of the players.");
            input = Console.ReadLine();

            while (!int.TryParse(input, out startingFunds))
            {
                Console.WriteLine("\nInput: \"" + input + "\" is not a valid integer. Please enter a valid integer for the starting funds of the players.");
                input = Console.ReadLine();
            }

            Console.WriteLine("\nPlease enter the minimum bet of the players.");
            input = Console.ReadLine();

            while (!int.TryParse(input, out minimumBet))
            {
                Console.WriteLine("\nInput: \"" + input + "\" is not a valid integer. Please enter a valid integer for the minimum bet of the players.");
                input = Console.ReadLine();
            }

            Console.WriteLine("\nPlease choose a betting strategy: \n\t1 - Manhattan\n\t2 - Martingale");
            input = Console.ReadLine();

            while (!int.TryParse(input, out bettingStrategy))
            {
                Console.WriteLine("\nInput: \"" + input + "\" is not a valid integer. Please enter a valid integer (1 or 2) for a betting strategy.");
                input = Console.ReadLine();
            }

            shoe = new Shoe(numberOfDecksInShoe);
            counter = new HiLoCounter(numberOfDecksInShoe);

            for (int i=0; i<numberOfPlayers; i++)
            {
                if(bettingStrategy == 1)
                {
                    playerList.Add(new Player(typeof(Manhattan), startingFunds, minimumBet));
                }
                else
                {
                    playerList.Add(new Player(typeof(Martingale), startingFunds, minimumBet));
                }                
            }
        }

        #endregion

    }
}
