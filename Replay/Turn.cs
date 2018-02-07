using Newtonsoft.Json;

namespace WizzardExtreme.Replay
{
    public class Turn
    {
        public int StartingPlayer;
        public int WinningPlayer;
        public Color Trick;
        public Card[] Cards;

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
