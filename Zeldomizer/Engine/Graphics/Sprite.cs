﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Engine.Graphics.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Graphics
{
    public class Sprite : ISprite
    {
        private readonly IRom _rom;
        private readonly int _offset;

        public Sprite(IRom rom, int offset)
        {
            _rom = rom;
            _offset = offset;
        }

        private IEnumerable<int> GetPixels()
        {
            return Enumerable.Range(0, Width * Height)
                .Select(i => this[i]);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<int> GetEnumerator() => GetPixels().GetEnumerator();
        public int Width => 8;
        public int Height => 8;
        private int GetOffset(int index) => _offset + (index >> 3);
        private static int GetBit(int index) => 0x80 >> (index & 7);

        public int this[int index]
        {
            get
            {
                var offset = GetOffset(index);
                var bit = GetBit(index);
                return ((_rom[offset] & bit) != 0 ? 1 : 0) |
                    ((_rom[offset + 8] & bit) != 0 ? 2 : 0);
            }
            set
            {
                var offset = GetOffset(index);
                var bit = GetBit(index);

                var input1 = _rom[offset];
                var data = ~bit & input1;
                var newData1 = unchecked((byte) (data | ((value & 1) != 0 ? bit : 0)));
                _rom[offset] = newData1;

                var input2 = _rom[offset + 8];
                data = ~bit & input2;
                var newData2 = unchecked((byte) (data | ((value & 2) != 0 ? bit : 0)));
                _rom[offset + 8] = newData2;

            }
        }
    }
}
