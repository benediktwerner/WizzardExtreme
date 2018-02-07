using System;
using System.Collections.Generic;
using System.Linq;

namespace WizzardExtreme
{
    public abstract class Player
    {
        public CardStack Hand;
        public Game Game;
        public ColorCounter Tricks;
        public int Points;

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
        }

        public Color ReturnTrick(Color baseColor, bool wizzard)
        {
            var tricksToReturn = new List<Color>(3);

            if (Tricks.Contains(Color.White))
                tricksToReturn.Add(Color.White);

            if (wizzard && Tricks.Contains(Color.Red))
                tricksToReturn.Add(Color.Red);

            if (Tricks.Contains(baseColor))
                tricksToReturn.Add(baseColor);

            if (tricksToReturn.Count == 0)
                return Color.Black;
            else if (tricksToReturn.Count == 1)
                return tricksToReturn.First();
            else
                return ChooseTrickToRemove(tricksToReturn);
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

        public abstract Card GetCardToPlay(Color? baseColor, Card winningCard);

        public override string ToString()
        {
            return Name;
        }
    }
}
