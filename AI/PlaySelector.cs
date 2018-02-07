using System.Collections.Generic;

namespace WizzardExtreme.AI
{
    public abstract class PlaySelector
    {
        internal Player Player { get; set; }

        public abstract Card RequestCard(Color? baseColor, Card winningCard);

        public virtual Color SelectTrickToRemove(IList<Color> trickColors)
        {
            // Default implementation
            // Always removes the first possible trick
            // Usually this should be WHITE > RED > COLOR
            return trickColors[0];
        }
    }
}
