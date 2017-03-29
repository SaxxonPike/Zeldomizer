using System.Collections;
using System.Collections.Generic;
using Zeldomizer.Engine.Graphics.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public class DungeonSpriteList : IEnumerable<ISprite>
    {
        private readonly IRom _rom;

        public DungeonSpriteList(IRom rom)
        {
            _rom = rom;
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
