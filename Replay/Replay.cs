using WizzardExtreme.Game;

namespace WizzardExtreme.Replay
{
    public class Replay
    {
        public int TurnIndex { get; private set; }
        public int NextPlayer { get; private set; }
        public CardStack Stack { get; private set; }
        public Player[] Players { get; private set; }

        private GameLog log;
        private Turn currentTurn;
        private bool tricksUpdated;

        public Replay(GameLog gameLog)
        {
            log = gameLog;
            GoToTurn(0);
        }

        public void GoToTurn(int turn)
        {
            TurnIndex = turn;
            tricksUpdated = false;
            Stack = new CardStack();
            currentTurn = log.Turns[turn];
            NextPlayer = currentTurn.StartingPlayer;

            Players = new Player[log.PlayerCount];
            for (int i = 0; i < Players.Length; i++)
            {
                Player p = log.Players[i];
                Players[i] = new Player(
                        p.Name,
                        new CardStack(p.Hand),
                        new ColorCounter(p.Tricks));
            }

            foreach (Turn t in log.Turns.GetRange(0, turn))
            {
                for (int i = 0; i < log.PlayerCount; i++)
                {
                    Players[i].Hand.Remove(t.Cards[i]);
                }
                Players[t.WinningPlayer].ProcessTrick(t.Trick);
            }
        }

        public void Next()
        {
            if (NextPlayer == currentTurn.StartingPlayer && Stack.Count != 0)
            {
                if (tricksUpdated)
                {
                    tricksUpdated = false;
                    Stack = new CardStack();
                    currentTurn = log.Turns[TurnIndex++];
                    NextPlayer = currentTurn.StartingPlayer;
                }
                else
                {
                    Players[currentTurn.WinningPlayer].ProcessTrick(currentTurn.Trick);
                    tricksUpdated = true;
                }
            }
            else
            {
                Card card = currentTurn.Cards[NextPlayer];
                Stack.Add(card);
                Players[NextPlayer].Hand.Remove(card);
                NextPlayer = (NextPlayer + 1) % log.PlayerCount;
            }
        }

        public static Replay LoadReplayFromLog(string path) {
            return new Replay(GameLog.LoadLog(path));
        }
    }
}
