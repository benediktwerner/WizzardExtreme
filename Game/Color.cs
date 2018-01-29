using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace WizzardExtreme.Game
{
    [JsonConverter(typeof(ColorConverter))]
    public class Color : IComparable<Color>
    {
        public static readonly Color Red = new Color(0, "Red", 2, System.Drawing.Color.FromArgb(220,0,0));
        public static readonly Color Green = new Color(1, "Green", 2, System.Drawing.Color.LimeGreen);
        public static readonly Color Blue = new Color(2, "Blue", 2, System.Drawing.Color.DeepSkyBlue);
        public static readonly Color Yellow = new Color(3, "Yellow", 2, System.Drawing.Color.Gold);
        public static readonly Color Violet = new Color(4, "Violet", 2, System.Drawing.Color.Magenta);
        public static readonly Color Black = new Color(5, "Black", 3, System.Drawing.Color.Black);
        public static readonly Color White = new Color(6, "White", 4, System.Drawing.Color.White);

        public const int Count = 7;

        public int Index { get; }
        public string Name { get; }
        public int Points { get; }
        public System.Drawing.Color DrawColor { get; }
        public bool IsCardColor => Index < 5;

        private Color(int index, string name, int points, System.Drawing.Color drawColor)
        {
            Index = index;
            Name = name;
            Points = points;
            DrawColor = drawColor;
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(Color other)
        {
            return Index.CompareTo(other.Index);
        }

        public readonly static Color[] Colors = new Color[]
        {
            Red,
            Green,
            Blue,
            Yellow,
            Violet,
            Black,
            White
        };

        public readonly static Color[] CardColors = new Color[]
        {
            Red,
            Green,
            Blue,
            Yellow,
            Violet
        };
    }

    class ColorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Color);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JValue t = JToken.ReadFrom(reader) as JValue;
            switch (t?.Value)
            {
                case "Red": return Color.Red;
                case "Green": return Color.Green;
                case "Blue": return Color.Blue;
                case "Yellow": return Color.Yellow;
                case "Violet": return Color.Violet;
                case "Black": return Color.Black;
                case "White": return Color.White;
                default: return null;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Color color = (Color)value;
            JToken.FromObject(color.Name).WriteTo(writer);
        }
    }
}
