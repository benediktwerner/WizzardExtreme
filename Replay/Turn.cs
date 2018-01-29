using Newtonsoft.Json;
using WizzardExtreme.Game;

namespace WizzardExtreme.Replay
{
    public class Turn
    {
        public int StartingPlayer { get; set; }
        public int WinningPlayer { get; set; }
        public Color Trick { get; set; }
        public Card[] Cards { get; set; }

        public Turn(int startingPlayer, int playerCount)
        {
            StartingPlayer = startingPlayer;
            Cards = new Card[playerCount];
        }

        [JsonConstructor]
        public Turn(int startingPlayer, int winningPlayer, Color trick, Card[] cards)
        {
            StartingPlayer = startingPlayer;
            WinningPlayer = winningPlayer;
            Trick = trick;
            Cards = cards;
        }

        public void AddCard(int player, Card card)
        {
            Cards[player] = card;
        }
    }
}
