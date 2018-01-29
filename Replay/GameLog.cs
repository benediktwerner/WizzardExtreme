using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using WizzardExtreme.Game;

namespace WizzardExtreme.Replay
{
    public class GameLog
    {
        public int StartingPlayer { get; set; }
        public int PlayerCount { get; set; }
        public Player[] Players { get; set; }
        public List<Turn> Turns { get; set; }

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

            g.AddTurn(new Turn(0, 1, Color.Green,
                    new Card[] {
                        new Card(2, Color.Green),
                        new Card(3, Color.Red)
                    }));
            g.SaveLog();
        }

        GameLog() { }

        public GameLog(int startingPlayer, int playerCount, Player[] players)
        {
            StartingPlayer = startingPlayer;
            PlayerCount = playerCount;
            Turns = new List<Turn>();
            Players = players;
        }

        public GameLog(int startingPlayer, int playerCount, Game.Player[] players)
        {
            StartingPlayer = startingPlayer;
            PlayerCount = playerCount;
            Turns = new List<Turn>();
            Players = new Player[players.Length];
            for (int i = 0; i < players.Length; i++)
                Players[i] = new Player(players[i]);
        }

        public void AddTurn(Turn turn)
        {
            Turns.Add(turn);
        }

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
