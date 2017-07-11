using System.Collections;
using System.Collections.Generic;
using Zeldomizer.Engine.Graphics.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public class DungeonSpriteList : IEnumerable<ISprite>
    {
        private readonly ISource _source;

        public DungeonSpriteList(ISource source)
        {
            _source = source;
        }

        public IEnumerator<ISprite> GetEnumerator()
        {
            yield break;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
