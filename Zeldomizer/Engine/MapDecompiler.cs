using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Engine
{
    public abstract class MapDecompiler
    {
        protected abstract int RoomWidth { get; }
        protected abstract int RoomHeight { get; }
    }
}
