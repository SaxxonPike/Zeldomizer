﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Zeldomizer.Engine
{
    public class TestPlayground : BaseTestFixture
    {
        [Test]
        public void Test1()
        {
            var cartridge = new ZeldaCartridge(Rom);

        }
    }
}