using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Underworld
{
    public class UnderworldLevelTests : ZeldomizerBaseTestFixture<UnderworldLevel>
    {
        private ISource _source;
        
        protected override UnderworldLevel GetTestSubject()
        {
            _source = new Source(0xFC);
            return new UnderworldLevel(_source);
        }

        [Test]
        public void LevelNumber_CanBeRead()
        {
            // Arrange.
            var value = Random<byte>();
            _source[0x30] = value;
            
            // Assert.
            Subject.LevelNumber.Should().Be(value);
        }

        [Test]
        public void LevelNumber_CanBeWritten()
        {
            // Arrange.
            var value = Random<byte>();
            
            // Act.
            Subject.LevelNumber = value;

            // Assert.
            _source[0x30].Should().Be(value);
        }
    }
}
