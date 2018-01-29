using Newtonsoft.Json;
using WizzardExtreme.Game;

namespace WizzardExtreme.Replay
{
    public class Player
    {
        public string Name { get; set; }
        public CardStack Hand { get; set; }
        public ColorCounter Tricks { get; set; }

        [JsonConstructor]
        public Player(string name, CardStack hand, ColorCounter tricks)
        {
            Name = name;
            Hand = hand;
            Tricks = tricks;
        }

        public Player(Game.Player player)
        {
            Name = player.Name;
            Hand = new CardStack(player.Hand);
            Tricks = new ColorCounter(player.Tricks);
        }

        public void ProcessTrick(Color color)
        {
            if (color == Color.Black)
                Tricks[Color.Black]++;
            else Tricks[color]++;
        }
    }
}
