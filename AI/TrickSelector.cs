using System.Collections.Generic;

namespace WizzardExtreme.AI
{
    public abstract class TrickSelector
    {
        internal Player Player { get; set; }

        public abstract IList<Color> SelectTricks(CardStack hand);
    }
}
