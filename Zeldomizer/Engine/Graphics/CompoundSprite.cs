using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Engine.Graphics.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public class CompoundSprite : ISprite
    {
        private readonly int _vertical;
        private readonly Sprite[] _sprites;
        private readonly int _total;

        public CompoundSprite(ISource source, int horizontal, int vertical, int offset)
        {
            _vertical = vertical;
            Width = horizontal * 8;
            Height = vertical * 8;
            _total = Width * Height;
            _sprites = Enumerable
                .Range(0, horizontal * vertical)
                .Select(i => new Sprite(source, offset + i * 16))
                .ToArray();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<int> GetEnumerator() => Enumerable
            .Range(0, _total).Select(i => this[i]).GetEnumerator();

        private int GetSpriteNumber(int index)
        {
            var wrappedIndex = index % _total;
            var x = wrappedIndex % Width;
            var y = wrappedIndex / Width;
            return (x >> 3) * _vertical + (y >> 3);
        }

        private int GetSpriteIndex(int index)
        {
            var wrappedIndex = index % _total;
            var x = (wrappedIndex % Width) & 7;
            var y = (wrappedIndex / Width) & 7;
            return x + (y << 3);
        }

        public int this[int index]
        {
            get => _sprites[GetSpriteNumber(index)][GetSpriteIndex(index)];
            set => _sprites[GetSpriteNumber(index)][GetSpriteIndex(index)] = value;
        }

        public int Width { get; }
        public int Height { get; }
    }
}
