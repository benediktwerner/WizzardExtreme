using System.Collections.Generic;
using WizzardExtreme.Game;

namespace WizzardExtreme.AI
{
    public abstract class PlaySelector
    {
        internal Player Player { get; set; }

        public abstract Card RequestCard(Color baseColor, Card winningCard);

        /**
         * Always removes the first possible trick
         * Usually this should be WHITE > RED > COLOR
         */
        public Color SelectTrickToRemove(IList<Color> trickColors)
        {
            return trickColors[0];
        }
    }
}
