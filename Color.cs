using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace WizzardExtreme
{
    [JsonConverter(typeof(ColorJsonConverter))]
    public enum Color
    {
        Red, Green, Blue, Yellow, Violet, Black, White
    }

    public static class ColorHelper
    {
        public const int ColorCount = 7;

        public readonly static Color[] Colors = new Color[]
        {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Yellow,
            Color.Violet,
            Color.Black,
            Color.White
        };

        public readonly static Color[] CardColors = new Color[]
        {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Yellow,
            Color.Violet
        };

        public static string GetName(this Color color)
        {
            return Enum.GetName(typeof(Color), color);
        }

        public static int GetPoints(this Color color)
        {
            switch (color)
            {
                case Color.Red:
                case Color.Green:
                case Color.Blue:
                case Color.Yellow:
                case Color.Violet:
                    return 2;
                case Color.Black:
                    return 3;
                case Color.White:
                    return 4;
            }
            throw new ArgumentException("Unknown Color: " + color.GetName());
        }

        public static bool IsCardColor(this Color color)
        {
            switch (color)
            {
                case Color.Red:
                case Color.Green:
                case Color.Blue:
                case Color.Yellow:
                case Color.Violet:
                    return true;
                case Color.Black:
                case Color.White:
                    return false;
            }
            throw new ArgumentException("Unknown Color: " + color.GetName());
        }
    }

    class ColorJsonConverter : JsonConverter
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
            JToken.FromObject(((Color)value).GetName()).WriteTo(writer);
        }
    }
}
