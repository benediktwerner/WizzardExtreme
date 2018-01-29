using System;
using WizzardExtreme.Replay;

namespace WizzardExtreme.Game
{
    public class Game
    {
        private ColorCounter tricks;
        public Player[] Players { get; private set; }
        public int PlayerCount { get; private set; }
        public int NextPlayer { get; private set; }

        public int HighestCardValue => PlayerCount * 3;

        public delegate void CardPlayedHandler(int playerId, Card card);
        public delegate void RoundFinishedHandler(int winningPlayerId, Color trickRemoved);

        public event CardPlayedHandler CardPlayed;
        public event RoundFinishedHandler RoundFinished;

        public static void Main2(String[] args)
        {
            Player[] players = {
                new HumanPlayer("Benedikt"),
                new HumanPlayer("Quirin"),
                new HumanPlayer("Anna"),
                new HumanPlayer("Josef")
            };

            Game game = new Game(players);
            GameRecorder recorder = new GameRecorder();

            game.SetupRound(0);
            recorder.StartRecording(game);
            //game.RoundFinished += (a,b) => recorder.SaveRecording();
            game.StartRound();

            Console.WriteLine("\nResults:");
            foreach (Player player in players)
                Console.WriteLine("Player " + player.Name + ": " + player.Points);
            recorder.SaveRecording();
        }

        public Game(Player[] players)
        {
            Players = players;
            PlayerCount = players.Length;
            foreach (Player player in players)
                player.Game = this;
        }

        public void SetupRound(int firstPlayer)
        {
            CardStack stack = CardStack.GetCompleteStack(HighestCardValue);
            stack.Shuffle();
            foreach (Player player in Players)
                player.GiveHand(stack.DrawCards(15));

            tricks = ColorCounter.GetTrickColors();
            NextPlayer = firstPlayer;

            Console.WriteLine(tricks);
        }

        // TODO: Check consistency
        public void StartRound()
        {
            foreach (Player player in Players)
                player.RequestTakeTricks();

            for (int r = 0; r < 15; r++)
            {
                Console.WriteLine("\nRound " + (r + 1));
                Console.WriteLine(Players[NextPlayer] + " starts!");
                DoTurn();
            }
            foreach (Player player in Players)
                player.ComputePoints();
        }

        private void DoTurn()
        {
            int player = NextPlayer;
            Card winningCard = null;
            Color baseColor = null;

            for (int i = 0; i < PlayerCount; i++)
            {
                Card card = Players[player].RequestCard(baseColor, winningCard);
                OnCardPlayed(player, card);

                if (baseColor == null)
                    baseColor = card.Color;
                if (card.BetterThan(winningCard, baseColor))
                {
                    winningCard = card;
                    NextPlayer = player;
                }
                player = (player + 1) % Players.Length;
            }

            Console.WriteLine(Players[NextPlayer] + " wins with " + winningCard + "!");

            Color trick = Players[NextPlayer].RequestTrick(baseColor, winningCard.IsWizzard);
            Players[NextPlayer].ProcessTrick(trick);
            OnRoundFinished(NextPlayer, trick);
        }

        public bool RequestTrick(Color trickColor)
        {
            if (!trickColor.IsCardColor)
                return false;
            if (tricks.Contains(trickColor))
            {
                tricks[trickColor]--;
                return true;
            }
            if (tricks.Contains(Color.White))
            {
                foreach (Player player in Players)
                    if (player.HasTrick(trickColor))
                    {
                        player.ReplaceWithWhite(trickColor);
                        return true;
                    }
            }
            return false;
        }

        public void OnCardPlayed(int playerId, Card card)
        {
            Console.WriteLine(Players[playerId] + " plays " + card);
            CardPlayed?.Invoke(playerId, card);
        }

        public void OnRoundFinished(int winningPlayerId, Color trickRemoved)
        {
            RoundFinished?.Invoke(winningPlayerId, trickRemoved);
        }
    }
}
