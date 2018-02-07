using Newtonsoft.Json;
using System;

namespace WizzardExtreme
{
    public class Card : IComparable<Card>
    {
        public int Number { get; }
        public Color Color { get; }

        [JsonIgnore]
        public bool IsWizzard => Color == Color.Red;

        public Card(int number, Color color)
        {
            Number = number;
            Color = color;
        }

        public bool BetterThan(Card winningCard)
        {
            return winningCard == null
                    || (IsWizzard && (!winningCard.IsWizzard || winningCard.Number < Number))
                    || (winningCard.Color == Color && winningCard.Number < Number);
        }

        public override string ToString()
        {
            return Color + " " + Number;
        }

        public int CompareTo(Card other)
        {
            if (Color == other.Color)
                return Number.CompareTo(other.Number);
            else
                return Color.CompareTo(other.Color);
        }
    }
}
