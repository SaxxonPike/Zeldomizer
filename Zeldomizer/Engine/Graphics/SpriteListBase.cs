using System.Collections;
using System.Collections.Generic;
using Zeldomizer.Engine.Graphics.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public abstract class SpriteListBase : IEnumerable<ISprite>
    {
        private readonly IRom _rom;
        private readonly int _listOffset;

        protected SpriteListBase(IRom rom, int offset)
        {
            _rom = rom;
            _listOffset = offset;
        }

        protected ISprite GetSprite(int index, int width, int height)
        {
            var spriteOffset = index << 4;
            if (width == 1 && height == 1)
                return new Sprite(_rom, _listOffset + spriteOffset);
            return new CompoundSprite(_rom, width, height, _listOffset + spriteOffset);
        }

        protected abstract IEnumerable<ISprite> GetSprites();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<ISprite> GetEnumerator() => GetSprites().GetEnumerator();
    }
}
