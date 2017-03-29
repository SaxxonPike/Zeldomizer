﻿using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Music
{
    public class MusicPointers : ByteList
    {
        public MusicPointers(IRom source) : base(new RomBlock(source, 0x00D60), 0x24)
        {
        }
    }
}
