﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Engine.Underworld
{
    public enum UnderworldExitType
    {
        OpenDoor = 0,
        Wall = 1,
        SecretFakeWall = 2,
        FakeWall = 3,
        Bombable = 4,
        LockedDoor = 5,
        Unknown = 6,
        Shutters = 7,
    }
}
