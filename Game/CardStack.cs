using System;
using System.Collections.Generic;

namespace WizzardExtreme.Game
{
    public class CardStack : List<Card>
    {
        public CardStack() : base() { }
        public CardStack(IEnumerable<Card> collection) : base(collection) { }

        public Card DrawCard()
        {
            return Remove(Count - 1);
        }

        public CardStack DrawCards(int count)
        {
            var cards = new CardStack();
            for (int i = 1; i <= count; i++)
                cards.Add(this[Count - i]);
            RemoveRange(Count - count - 1, count);
            return cards;
        }

        public Card Remove(int index)
        {
            Card c = this[index];
            RemoveAt(index);
            return c;
        }

        public void Shuffle()
        {
            var originalStack = new List<Card>(this);
            Random random = new Random();
            for (int i = originalStack.Count; i > 0; i--)
            {
                int rand = random.Next(i);
                Add(originalStack[rand]);
                originalStack.RemoveAt(rand);
            }
        }

        public static CardStack GetCompleteStack(int maxNumber)
        {
            CardStack stack = new CardStack();
            foreach (var color in Color.CardColors)
                for (int i = 1; i <= maxNumber; i++)
                    stack.Add(new Card(i, color));
            return stack;
        }

        public override String ToString()
        {
            string s = "";
            for (int i = 0; i < Count; i++)
                s += i + ":\t" + this[i] + (i != Count - 1 ? "\n" : "");
            return s;
        }
    }
}
