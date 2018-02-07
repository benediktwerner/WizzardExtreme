using System;
using System.Collections.Generic;

namespace WizzardExtreme
{
    public class HumanConsolePlayer : Player
    {
        public HumanConsolePlayer(string name) : base(name) { }

        public override void RequestTakeTricks()
        {
            Console.WriteLine("\n" + Name);
            Console.WriteLine("Your hand:");
            Console.WriteLine(Hand);
            Console.WriteLine("Choose your tricks (-1 to stop):");
            foreach (var color in ColorHelper.CardColors)
                Console.WriteLine((int)color + ": " + color);
            while (true)
            {
                int input = int.Parse(Console.ReadLine());
                if (input == -1)
                    return;
                else if (input >= 0 && input < 5)
                {
                    Color color = ColorHelper.Colors[input];
                    if (RequestTrick(color))
                        Console.WriteLine("You took a " + color + " trick!");
                    else
                        Console.WriteLine("You can't take anymore " + color + " tricks!");
                }
                else
                    Console.WriteLine("Invalid choice!");
            }
        }

        public override Card GetCardToPlay(Color? baseColor, Card winningCard)
        {
            Console.WriteLine("\n" + Name);
            Console.WriteLine("Your tricks:");
            Console.WriteLine(Tricks);
            Console.WriteLine("Your hand:");
            Console.WriteLine(Hand);
            if (baseColor == null)
                Console.WriteLine("You go first");
            else
            {
                Console.WriteLine("Current color: " + baseColor);
                Console.WriteLine("Winning card: " + winningCard);
            }
            Console.WriteLine("Choose a card:");
            while (true)
            {
                int card = int.Parse(Console.ReadLine());
                if (card >= 0 && card < Hand.Count)
                    return Hand.Remove(card);
                else
                    Console.WriteLine("Invalid choice!");
            }
        }

        public override Color ChooseTrickToRemove(IList<Color> trickColors)
        {
            Console.WriteLine("\n" + Name);
            Console.WriteLine("Your tricks:");
            Console.WriteLine(Tricks);
            Console.WriteLine("You can remove a trick:");
            int i = 0;
            foreach (Color color in trickColors)
                Console.WriteLine(i++ + ": " + color);
            while (true)
            {
                int input = int.Parse(Console.ReadLine());
                if (input >= 0 && input < i)
                    return trickColors[input];
                else
                    Console.WriteLine("Invalid choice!");
            }
        }
    }
}
