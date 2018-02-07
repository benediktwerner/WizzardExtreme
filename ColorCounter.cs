using System;
using Newtonsoft.Json;

namespace WizzardExtreme
{
    public class ColorCounter
    {
        [JsonProperty]
        private int[] colors;

        public int this[Color c]
        {
            get => colors[(int)c];
            set => colors[(int)c] = value;
        }

        public ColorCounter(ColorCounter colorCounter) : this()
        {
            colorCounter.colors.CopyTo(colors, 0);
        }

        [JsonConstructor]
        public ColorCounter(int[] colors = null)
        {
            this.colors = colors ?? new int[ColorHelper.ColorCount];
        }

        public int GetPoints()
        {
            int points = 0;
            foreach (var color in ColorHelper.Colors)
                points += this[color] * color.GetPoints();
            return points;
        }

        public bool Contains(Color color)
        {
            return this[color] > 0;
        }

        public override string ToString()
        {
            string s = "";
            foreach (var color in ColorHelper.Colors)
                if (Contains(color))
                    s += color + ": " + this[color] + "\n";
            return s;
        }

        public static ColorCounter GetTrickColorsCounter()
        {
            ColorCounter trickColors = new ColorCounter();
            foreach (var color in ColorHelper.CardColors)
                trickColors[color] = 3;

            trickColors[Color.Red] = 5;
            trickColors[Color.White] = 4;
            trickColors[Color.Black] = 6;
            return trickColors;
        }
    }
}
