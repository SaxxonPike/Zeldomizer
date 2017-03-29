using System.Collections;
using System.Collections.Generic;
using Zeldomizer.Engine.Graphics.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public abstract class SpriteListBase : IEnumerable<ISprite>
    {
        private readonly ISource _source;
        private readonly int _listOffset;

        protected SpriteListBase(ISource source, int offset)
        {
            _source = source;
            _listOffset = offset;
        }

        protected ISprite GetSprite(int index, int width, int height)
        {
            var spriteOffset = index << 4;
            if (width == 1 && height == 1)
                return new Sprite(_source, _listOffset + spriteOffset);
            return new CompoundSprite(_source, width, height, _listOffset + spriteOffset);
        }

        protected abstract IEnumerable<ISprite> GetSprites();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<ISprite> GetEnumerator() => GetSprites().GetEnumerator();
    }
}
