using System;
using System.Collections.Generic;
using System.Linq;

namespace WizzardExtreme.Game
{
    public abstract class Player
    {
        public Game Game { get; internal set; }
        public CardStack Hand { get; private set; }
        public ColorCounter Tricks { get; private set; }
        public int Points { get; private set; }

        private event Action<CardStack> HandGiven;

        public string Name { get; }

        public Player(string name)
        {
            Name = name;
            Points = 0;
            Tricks = new ColorCounter();
        }

        public void GiveHand(CardStack hand)
        {
            Hand = hand;
            Hand.Sort();
            HandGiven?.Invoke(Hand);
        }

        public void ReplaceWithWhite(Color color)
        {
            Tricks[color]--;
            Tricks[Color.White]++;
            Console.WriteLine(Name + " got a white instead of " + color);
        }

        public void ComputePoints()
        {
            Points += Tricks.GetPoints();
        }

        public void ProcessTrick(Color color)
        {
            if (color == Color.Black)
            {
                Tricks[Color.Black]++;
                Console.WriteLine(Name + " got a black trick!");
            }
            else
            {
                Tricks[color]--;
                Console.WriteLine(Name + " removed a " + color + " trick!");
            }
        }

        public Color RequestTrick(Color baseColor, bool wizzard)
        {
            var trickColors = new HashSet<Color>();
            if (HasTrick(Color.White))
                trickColors.Add(Color.White);
            if (wizzard && HasTrick(Color.Red))
                trickColors.Add(Color.Red);
            if (HasTrick(baseColor))
                trickColors.Add(baseColor);

            if (trickColors.Count == 0)
                return Color.Black;
            else if (trickColors.Count == 1)
                return trickColors.First();
            else
                return ChooseTrickToRemove(new List<Color>(trickColors));
        }

        public abstract Color ChooseTrickToRemove(IList<Color> trickColors);

        public abstract void RequestTakeTricks();

        protected bool RequestTrick(Color color)
        {
            if (Game.RequestTrick(color))
            {
                Tricks[color]++;
                return true;
            }
            return false;
        }

        public abstract Card RequestCard(Color baseColor, Card winningCard);

        public bool HasTrick(Color color)
        {
            return Tricks.Contains(color);
        }

        public int GetTricks(Color color)
        {
            return Tricks[color];
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
