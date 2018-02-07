using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace WizzardExtreme.Replay
{
    public class GameLog
    {
        public int StartingPlayer;
        public int PlayerCount;
        public Player[] Players;
        public List<Turn> Turns;

        internal static void TestWrite()
        {
            ColorCounter c1 = new ColorCounter();
            c1[Color.Red]++;
            ColorCounter c2 = new ColorCounter();
            c2[Color.Green]++;
            c2[Color.Blue]++;

            Player[] players = {
                new Player("Hans", new CardStack(new Card[] {new Card(3, Color.Red), new Card(6, Color.Red)}), c1),
                new Player("Peter", new CardStack(new Card[] {new Card(2, Color.Green), new Card(7, Color.Blue)}), c2)
            };
            GameLog g = new GameLog(2, 3, players);

            g.Turns.Add(new Turn(0, 1, Color.Green,
                    new Card[] {
                        new Card(2, Color.Green),
                        new Card(3, Color.Red)
                    }));
            g.SaveLog();
        }

        GameLog() { }

        public GameLog(int startingPlayer, int playerCount, IEnumerable<Player> players)
        {
            StartingPlayer = startingPlayer;
            PlayerCount = playerCount;
            Turns = new List<Turn>();
            Players = players.ToArray();
        }

        public GameLog(int startingPlayer, int playerCount, WizzardExtreme.Player[] players)
            : this(startingPlayer, playerCount, players.Select(p => new Player(p)))
        { }

        public void SaveLog(string path = null)
        {
            if (path == null)
                path = $"game_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.json";

            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public static GameLog LoadLog(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<GameLog>(json); ;
        }
    }
}
