using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldLevelTests : ZeldomizerBaseTestFixture<UnderworldLevel>
    {
        protected override UnderworldLevel GetTestSubject()
        {
            var cart = new ZeldaCartridge(Source);
            return cart.Underworld.Levels[2];
        }

        [Test]
        public void Test1()
        {
            var room = new ZeldaCartridge(Source).Underworld.Grids[0][0x00];
            Subject.LevelNumber.Should().Be(9);
        }
    }
}
