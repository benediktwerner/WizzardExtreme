using System;
using System.Collections.Generic;
using System.Linq;
using WizzardExtreme.Game;

namespace WizzardExtreme.AI
{
    public class SimpleTrickSelector : TrickSelector
    {
        public static void Main2(string[] args)
        {
            Console.WriteLine("Enter cards:");
            CardStack hand = new CardStack();
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "e")
                    break;

                Color col;
                switch (input[0])
                {
                    case 'g': col = Color.Green; break;
                    case 'y': col = Color.Yellow; break;
                    case 'r': col = Color.Red; break;
                    case 'b': col = Color.Blue; break;
                    case 'l': col = Color.Violet; break;
                    default: Console.WriteLine("Invalid color"); continue;
                }
                hand.Add(new Card(int.Parse(input.Substring(1)), col));
            }

            var tricks = new SimpleTrickSelector().SelectTricks(hand);
            foreach (Color trick in tricks)
                Console.WriteLine(trick);
        }

        public override IList<Color> SelectTricks(CardStack hand)
        {
            List<Color> tricks = new List<Color>();
            int highestValue = Player.Game.HighestCardValue;

            foreach (Color color in Color.CardColors)
            {
                int[] values = hand
                    .Where(c => c.Color == color)
                    .Select(c => c.Number)
                    .OrderBy(c => c)
                    .ToArray();

                if (values.Length == 0)
                    continue;

                int highestHandIndex = values.Length - 1;
                while (highestHandIndex >= 0 && values[highestHandIndex] > highestValue - 3)
                {
                    int value = values[highestHandIndex--];
                    if (values.Length > highestValue - value)
                        tricks.Add(color);
                }
            }
            return tricks;
        }
    }
}
