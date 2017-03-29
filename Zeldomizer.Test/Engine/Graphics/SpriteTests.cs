using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Zeldomizer.Engine.Graphics
{
    public class SpriteTests : BaseTestFixture
    {
        [Test]
        public void Sprite_ReadsCorrectData()
        {
            var expected = new[]
            {
                0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 1, 1, 1,
                0, 0, 0, 1, 1, 1, 1, 1,
                0, 1, 1, 1, 2, 1, 1, 3,
                1, 1, 1, 1, 2, 2, 3, 3,
                1, 0, 1, 1, 2, 2, 2, 3,
                0, 0, 1, 3, 3, 2, 2, 3,
                0, 0, 0, 3, 3, 3, 2, 2
            };

            var sprite = new Sprite(Source, 0x807F);
            sprite.ToArray().Should().Equal(expected);
        }

        [Test]
        public void Sprite_WritesCorrectData([Values(0, 1, 2, 3)] int pixelValue, [Random(0x00, 0x3F, 3)] int index)
        {
            var expected = new[]
            {
                0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 1, 1, 1,
                0, 0, 0, 1, 1, 1, 1, 1,
                0, 1, 1, 1, 2, 1, 1, 3,
                1, 1, 1, 1, 2, 2, 3, 3,
                1, 0, 1, 1, 2, 2, 2, 3,
                0, 0, 1, 3, 3, 2, 2, 3,
                0, 0, 0, 3, 3, 3, 2, 2
            };

            expected[index] = pixelValue;

            var sprite = new Sprite(Source, 0x807F) {[index] = pixelValue};

            sprite.ToArray().Should().Equal(expected);
        }
    }
}
