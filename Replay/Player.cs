using Newtonsoft.Json;

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

        public Player(Player player) : this(player.Name, new CardStack(player.Hand), new ColorCounter(player.Tricks)) { }

        public Player(WizzardExtreme.Player player)
        {
            Name = player.Name;
            Hand = new CardStack(player.Hand);
            Tricks = new ColorCounter(player.Tricks);
        }
    }
}
