using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Engine.Graphics.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public class OverworldSpriteList : SpriteListBase
    {
        public OverworldSpriteList(ISource source) : base(source, 0)
        {
        }

        protected override IEnumerable<ISprite> GetSprites()
        {
            return Enumerable
                .Range(0, 130)
                .Select(i => GetSprite(i, 1, 1));
        }
    }
}
