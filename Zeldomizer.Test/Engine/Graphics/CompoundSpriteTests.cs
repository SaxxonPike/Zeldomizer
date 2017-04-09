using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Zeldomizer.Engine.Graphics
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class CompoundSpriteTests : ZeldomizerBaseTestFixture
    {
        [Test]
        public void CompoundSprite_ReadsCorrectData()
        {
            var expected = new[]
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 1, 1, 1, 1, 1, 3, 3, 3, 3, 0, 0, 0, 0,
                0, 1, 1, 1, 2, 1, 1, 3, 3, 3, 3, 3, 3, 0, 0, 0,
                1, 1, 1, 1, 2, 2, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0,
                1, 0, 1, 1, 2, 2, 2, 3, 2, 2, 1, 2, 0, 0, 0, 0,
                0, 0, 1, 3, 3, 2, 2, 3, 2, 2, 3, 2, 2, 2, 0, 0,
                0, 0, 0, 3, 3, 3, 2, 2, 2, 2, 2, 2, 0, 3, 0, 0,
                0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 0, 3, 0, 0,
                0, 0, 1, 3, 3, 1, 1, 2, 2, 2, 3, 3, 2, 3, 0, 0,
                0, 0, 3, 3, 3, 3, 3, 2, 2, 2, 1, 3, 2, 3, 0, 0,
                0, 1, 3, 3, 3, 3, 3, 2, 2, 1, 1, 3, 0, 3, 0, 0,
                0, 1, 1, 3, 3, 3, 3, 1, 1, 1, 3, 0, 0, 3, 0, 0,
                3, 3, 1, 1, 1, 1, 1, 3, 3, 3, 3, 1, 0, 3, 0, 0,
                3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 0, 0, 0,
                0, 3, 3, 3, 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 0, 0
            };

            var sprite = new CompoundSprite(Source, 2, 2, 0x807F);
            sprite.ToArray().Should().Equal(expected);
        }

        [Test]
        public void CompoundSprite_WritesCorrectData([Values(0, 1, 2, 3)] int pixelValue, [Random(0x00, 0xFF, 10)] int index)
        {
            var expected = new[]
            {
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 1, 1, 1, 1, 1, 3, 3, 3, 3, 0, 0, 0, 0,
                0, 1, 1, 1, 2, 1, 1, 3, 3, 3, 3, 3, 3, 0, 0, 0,
                1, 1, 1, 1, 2, 2, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0,
                1, 0, 1, 1, 2, 2, 2, 3, 2, 2, 1, 2, 0, 0, 0, 0,
                0, 0, 1, 3, 3, 2, 2, 3, 2, 2, 3, 2, 2, 2, 0, 0,
                0, 0, 0, 3, 3, 3, 2, 2, 2, 2, 2, 2, 0, 3, 0, 0,
                0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 0, 3, 0, 0,
                0, 0, 1, 3, 3, 1, 1, 2, 2, 2, 3, 3, 2, 3, 0, 0,
                0, 0, 3, 3, 3, 3, 3, 2, 2, 2, 1, 3, 2, 3, 0, 0,
                0, 1, 3, 3, 3, 3, 3, 2, 2, 1, 1, 3, 0, 3, 0, 0,
                0, 1, 1, 3, 3, 3, 3, 1, 1, 1, 3, 0, 0, 3, 0, 0,
                3, 3, 1, 1, 1, 1, 1, 3, 3, 3, 3, 1, 0, 3, 0, 0,
                3, 3, 3, 1, 1, 1, 1, 1, 1, 1, 1, 3, 3, 0, 0, 0,
                0, 3, 3, 3, 0, 0, 0, 0, 0, 3, 3, 3, 0, 0, 0, 0
            };

            expected[index] = pixelValue;

            var sprite = new CompoundSprite(Source, 2, 2, 0x807F) { [index] = pixelValue };
            sprite.ToArray().Should().Equal(expected);
        }
    }
}
