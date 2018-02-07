using System.Linq;

namespace WizzardExtreme.Replay
{
    public class Replay
    {
        public int TurnIndex;
        public int NextPlayer;
        public CardStack Stack;
        public Player[] Players;

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

            Players = log.Players.Select(p => new Player(p)).ToArray();

            foreach (Turn t in log.Turns.GetRange(0, turn))
            {
                for (int i = 0; i < log.PlayerCount; i++)
                    Players[i].Hand.Remove(t.Cards[i]);
                
                Players[t.WinningPlayer].Tricks[t.Trick] += (t.Trick == Color.Black) ? +1 : -1;
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
                    Players[currentTurn.WinningPlayer].Tricks[currentTurn.Trick] += (currentTurn.Trick == Color.Black) ? +1 : -1;
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
