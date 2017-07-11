using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldLevelListTests : ZeldomizerBaseTestFixture<UnderworldLevelList>
    {
        protected override UnderworldLevelList GetTestSubject()
        {
            return new ZeldaCartridge(Source).Underworld.Levels;
        }

        [Test]
        public void Count_ShouldHaveCorrectCountOfLevels()
        {
            // Assert.
            Subject.Count.Should().Be(9);
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 4)]
        [TestCase(4, 5)]
        [TestCase(5, 6)]
        [TestCase(6, 7)]
        [TestCase(7, 8)]
        [TestCase(8, 9)]
        public void This_RetrievesCorrectLevels(int index, int expectedLevel)
        {
            // Assert.
            Subject[index].LevelNumber.Should().Be(expectedLevel);
        }
    }
}
