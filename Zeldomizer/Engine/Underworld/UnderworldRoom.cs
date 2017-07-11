using System.Linq;
using Zeldomizer.Engine.Underworld.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    /// <summary>
    /// Represents an underworld room's properties, in raw form.
    /// </summary>
    public class UnderworldRoom : IUnderworldRoom
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
        /// Get or set the exit type for the south wall.
        /// </summary>
        public UnderworldExitType ExitSouth
        {
            get => (UnderworldExitType) _source[0x000].Bits(4, 2);
            set => _source[0x000] = _source[0x000].Bits(4, 2, (int) value);
        }

        /// <summary>
        /// Get or set the exit type for the north wall.
        /// </summary>
        public UnderworldExitType ExitNorth
        {
            get => (UnderworldExitType)_source[0x000].Bits(7, 5);
            set => _source[0x000] = _source[0x000].Bits(7, 5, (int)value);
        }

        /// <summary>
        /// Get or set the exit type for the east wall.
        /// </summary>
        public UnderworldExitType ExitEast
        {
            get => (UnderworldExitType)_source[0x080].Bits(4, 2);
            set => _source[0x080] = _source[0x080].Bits(4, 2, (int)value);
        }

        /// <summary>
        /// Get or set the exit type for the west wall.
        /// </summary>
        public UnderworldExitType ExitWest
        {
            get => (UnderworldExitType)_source[0x080].Bits(7, 5);
            set => _source[0x080] = _source[0x080].Bits(7, 5, (int)value);
        }

        /// <summary>
        /// This value determines which monsters will be present in the room, or if messages are present. Values 00-7F.
        /// </summary>
        public int Monsters
        {
            get => _source[0x100].Bits(5, 0) | (_source[0x180].Bit(7) ? 0x40 : 0x00);
            set
            {
                _source[0x100] = _source[0x100].Bits(5, 0, value & 0x3F);
                _source[0x180] = _source[0x180].Bit(7, (value & 0x40) != 0);
            }
        }

        /// <summary>
        /// This value determines which of four monster arrangements will be used.
        /// </summary>
        public int MonsterArrangement
        {
            get => _source[0x100].Bits(7, 6);
            set => _source[0x100] = _source[0x100].Bits(7, 6, value);
        }

        /// <summary>
        /// Determines which room layout will be used.
        /// </summary>
        public int Layout
        {
            get => _source[0x180].Bits(5, 0);
            set => _source[0x180] = _source[0x180].Bits(5, 0, value);
        }

        /// <summary>
        /// If true, the room has a pushable block.
        /// </summary>
        public bool HasPushableBlock
        {
            get => _source[0x180].Bit(6);
            set => _source[0x180] = _source[0x180].Bit(6, value);
        }

        /// <summary>
        /// Determines which item will appear on the floor.
        /// </summary>
        public int FloorItem
        {
            get => _source[0x200].Bits(4, 0);
            set => _source[0x200] = _source[0x200].Bits(4, 0, value);
        }

        /// <summary>
        /// Determines which roar type to use. If zero, no roar is present in this room.
        /// </summary>
        public int RoarType
        {
            get => _source[0x200].Bits(6, 5);
            set => _source[0x200] = _source[0x200].Bits(6, 5, value);
        }

        /// <summary>
        /// Determines which item will appear on the floor.
        /// </summary>
        public ItemKind FloorItemKind
        {
            get => (ItemKind) FloorItem;
            set => FloorItem = (int) value;
        }

        /// <summary>
        /// If true, the room is dark.
        /// </summary>
        public bool Dark
        {
            get => _source[0x200].Bit(7);
            set => _source[0x200] = _source[0x200].Bit(7, value);
        }

        /// <summary>
        /// Determines which item will appear on a monster.
        /// </summary>
        public UnderworldRoomScript Script
        {
            get => (UnderworldRoomScript)_source[0x280].Bits(2, 0);
            set => _source[0x280] = _source[0x280].Bits(2, 0, (int)value);
        }

        /// <summary>
        /// Determines at which of four positions the item will drop.
        /// </summary>
        public int ItemDropPosition
        {
            get => _source[0x280].Bits(5, 4);
            set => _source[0x280] = _source[0x280].Bits(5, 4, value);
        }

        /// <summary>
        /// Get the string representation of this dungeon room.
        /// </summary>
        public override string ToString()
        {
            return string.Join(" ", Enumerable.Range(0, 6).Select(i => $"{_source[i * 0x080]:X2}"));
        }
    }
}
