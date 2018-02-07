using System;

namespace WizzardExtreme.Replay
{
    public class GameRecorder
    {
        private GameLog log;
        private Game game;
        private Turn currentTurn;

        public void StartRecording(Game game)
        {
            game.CardPlayed += OnCardPlayed;
            game.RoundFinished += OnRoundFinished;

            this.game = game;
            log = new GameLog(game.NextPlayer, game.Players.Length, game.Players);
            currentTurn = new Turn(log.StartingPlayer, log.PlayerCount);
        }

        public void SaveRecording(string path = null)
        {
            log.SaveLog(path);
        }

        public void StopRecording()
        {
            game.CardPlayed -= OnCardPlayed;
            game.RoundFinished -= OnRoundFinished;
        }

        public void OnRoundFinished(int winningPlayerId, Color trickRemoved)
        {
            currentTurn.WinningPlayer = winningPlayerId;
            currentTurn.Trick = trickRemoved;
            log.Turns.Add(currentTurn);
            currentTurn = new Turn(winningPlayerId, log.PlayerCount);
        }

        public void OnCardPlayed(int playerId, Card card)
        {
            currentTurn.AddCard(playerId, card);
        }
    }
}
