using System.Collections.Generic;
using System.Linq;
using Zeldomizer.Engine.Overworld;
using Zeldomizer.Engine.Overworld.Interfaces;
using Zeldomizer.Engine.Underworld;
using Zeldomizer.Engine.Underworld.Interfaces;

namespace Zeldomizer.Engine.AI
{
    public class Pathfinder
    {
        private readonly IOverworld _overworld;
        private readonly IUnderworld _underworld;

        public Pathfinder(IOverworld overworld, IUnderworld underworld)
        {
            _overworld = overworld;
            _underworld = underworld;
        }

        public void Start()
        {
            var overworldDecompiler = new OverworldRoomDecompiler();
            var overworldRooms = overworldDecompiler.Decompile(_overworld.ColumnLibraries, _overworld.RoomLayouts);
            var overworldMap = overworldRooms.Rooms.Select(r => r.ToArray()).ToArray();
            var underworldDecompiler = new UnderworldRoomDecompiler();
            var underworldRooms = underworldDecompiler.Decompile(_underworld.ColumnLibraries, _underworld.RoomLayouts);
            var underworldMap = underworldRooms.Rooms.Select(r => r.ToArray()).ToArray();

            bool OverworldPassable(int terrain)
            {
                return !(terrain >= 0x85 && terrain < 0xE5);
            }
        }
    }
}
