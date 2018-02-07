using System;

namespace WizzardExtreme
{
    public class Game
    {
        public ColorCounter Tricks;
        public Player[] Players;
        public int NextPlayer { get; private set; }

        public int HighestCardValue => Players.Length * 3;

        public delegate void CardPlayedHandler(int playerId, Card card);
        public delegate void RoundFinishedHandler(int winningPlayerId, Color trickRemoved);

        public event CardPlayedHandler CardPlayed;
        public event RoundFinishedHandler RoundFinished;

        // public static void Main(String[] args)
        // {
        //     Player[] players = {
        //         new HumanPlayer("Benedikt"),
        //         new HumanPlayer("Quirin"),
        //         new HumanPlayer("Anna"),
        //         new HumanPlayer("Josef")
        //     };

        //     Game game = new Game(players);
        //     GameRecorder recorder = new GameRecorder();

        //     game.SetupRound(0);
        //     recorder.StartRecording(game);
        //     //game.RoundFinished += (a,b) => recorder.SaveRecording();
        //     game.StartRound();

        //     Console.WriteLine("\nResults:");
        //     foreach (Player player in players)
        //         Console.WriteLine("Player " + player.Name + ": " + player.Points);
        //     recorder.SaveRecording();
        // }

        public Game(Player[] players)
        {
            Players = players;
            foreach (Player player in Players) {
                player.Game = this;
            }
        }

        public void SetupRound(int firstPlayer)
        {
            CardStack stack = CardStack.GetCompleteStack(HighestCardValue);
            stack.Shuffle();
            foreach (Player player in Players)
                player.GiveHand(stack.DrawCards(15));

            Tricks = ColorCounter.GetTrickColorsCounter();
            NextPlayer = firstPlayer;
        }

        public void StartRound()
        {
            foreach (Player player in Players)
                player.RequestTakeTricks();

            for (int turn = 1; turn <= 15; turn++)
            {
                Console.WriteLine("\nTurn " + turn);
                Console.WriteLine(Players[NextPlayer] + " starts!");
                PlayTurn();
            }

            ComputePoints();
        }

        private void ComputePoints()
        {
            foreach (Player player in Players)
                player.Points += player.Tricks.GetPoints();
        }

        private void PlayTurn()
        {
            int player = NextPlayer;
            Card winningCard = null;
            Color? baseColor = null;

            for (int i = 0; i < Players.Length; i++)
            {
                Card card = Players[player].GetCardToPlay(baseColor, winningCard);
                OnCardPlayed(player, card);

                if (baseColor == null)
                    baseColor = card.Color;

                if (card.BetterThan(winningCard))
                {
                    winningCard = card;
                    NextPlayer = player;
                }
                player = (player + 1) % Players.Length;
            }

            Console.WriteLine(Players[NextPlayer] + " wins with " + winningCard + "!");

            Color returnedTrick = Players[NextPlayer].ReturnTrick((Color)baseColor, winningCard.IsWizzard);
            if (returnedTrick != baseColor && (!winningCard.IsWizzard || returnedTrick != Color.Red))
                throw new InvalidOperationException(Players[NextPlayer] + " returned an illegal trick: " + returnedTrick + " with baseColor " + baseColor + " and winningCard " + winningCard);
            RoundFinished?.Invoke(NextPlayer, returnedTrick);
        }

        public bool RequestTrick(Color trickColor)
        {
            if (!trickColor.IsCardColor())
                return false;

            if (Tricks.Contains(trickColor))
            {
                Tricks[trickColor]--;
                return true;
            }

            if (Tricks.Contains(Color.White))
            {
                for (int i = 0; i < Players.Length; i++)
                {
                    Player player = Players[(NextPlayer + i) % Players.Length];
                    if (player.Tricks.Contains(trickColor))
                    {
                        player.Tricks[trickColor]--;
                        player.Tricks[Color.White]++;
                        return true;
                    }
                }
            }
            return false;
        }

        public void OnCardPlayed(int playerId, Card card)
        {
            Console.WriteLine(Players[playerId] + " plays " + card);
            CardPlayed?.Invoke(playerId, card);
        }
    }
}
