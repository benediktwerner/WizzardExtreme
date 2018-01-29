using System;
using System.Collections.Generic;
using WizzardExtreme.AI;

namespace WizzardExtreme.Game
{
    public class ComputerPlayer : Player
    {
        private readonly TrickSelector trickSelector;
        private readonly PlaySelector playSelector;

        public ComputerPlayer(string name, TrickSelector trickSelector, PlaySelector playSelector) : base(name)
        {
            this.trickSelector = trickSelector;
            this.playSelector = playSelector;

            trickSelector.Player = this;
            playSelector.Player = this;
        }

        public ComputerPlayer(string name) : this(name, new SimpleTrickSelector(), new SimplePlaySelector()) { }

        public override Color ChooseTrickToRemove(IList<Color> trickColors)
        {
            return playSelector.SelectTrickToRemove(trickColors);
        }

        public override Card RequestCard(Color baseColor, Card winningCard)
        {
            return playSelector.RequestCard(baseColor, winningCard);
        }

        public override void RequestTakeTricks()
        {
            foreach (Color trick in trickSelector.SelectTricks(Hand))
                if (!RequestTrick(trick))
                    Console.Error.WriteLine(Name + " requested an invalid trick: " + trick);
        }
    }
}
