﻿using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    /// <summary>
    /// Represents an underworld room's properties, in raw form.
    /// </summary>
    public class UnderworldRoom
    {
        private readonly ISource _source;

        /// <summary>
        /// Initialize an underworld room's properties.
        /// </summary>
        public UnderworldRoom(ISource source)
        {
            _source = source;
        }

        /// <summary>
        /// Outer color palette to use for the room.
        /// </summary>
        public int Color0
        {
            get => _source[0x000];
            set => _source[0x000] = unchecked((byte)value);
        }

        /// <summary>
        /// Inner color palette to use for the room.
        /// </summary>
        public int Color1
        {
            get => _source[0x080];
            set => _source[0x080] = unchecked((byte)value);
        }

        /// <summary>
        /// This value determines which monsters will be present in the room.
        /// </summary>
        public int Monsters
        {
            get => _source[0x100];
            set => _source[0x100] = unchecked((byte)value);
        }

        /// <summary>
        /// Determines which room layout will be used.
        /// </summary>
        public int Layout
        {
            get => _source[0x180].Bits(6, 0);
            set => _source[0x180] = _source[0x180].Bits(6, 0, value);
        }

        /// <summary>
        /// Unknown currently.
        /// </summary>
        public bool LayoutFlag
        {
            get => _source[0x180].Bit(7);
            set => _source[0x180] = _source[0x180].Bit(7, value);
        }

        /// <summary>
        /// Determines which item will appear on the floor.
        /// </summary>
        public int FloorItem
        {
            get => _source[0x200];
            set => _source[0x200] = unchecked((byte)value);
        }

        /// <summary>
        /// Determines which item will appear on a monster.
        /// </summary>
        public int SpecialItem
        {
            get => _source[0x280];
            set => _source[0x280] = unchecked((byte)value);
        }

        public override string ToString()
        {
            return string.Join(" ", Enumerable.Range(0, 6).Select(i => $"{_source[i * 0x080]:X2}"));
        }
    }
}
