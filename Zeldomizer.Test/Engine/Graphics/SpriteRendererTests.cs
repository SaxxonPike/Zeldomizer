using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Zeldomizer.Engine.Graphics
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class SpriteRendererTests : ZeldomizerBaseTestFixture<SpriteRenderer>
    {
        protected override SpriteRenderer GetTestSubject()
        {
            var result = new SpriteRenderer();

            // Assign random colors to the palette.
            for (var i = 0; i < 4; i++)
                result.Colors[i] = Color.FromArgb(Random<int>());
            return result;
        }

        [Test]
        public void Render_UsesCorrectColors()
        {
            // Arrange.
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
            }.Select(i => Subject.Colors[i].ToArgb()).ToArray();

            var sprite = new Sprite(Source, 0x807F);

            // Act.
            using (var renderedSprite = Subject.Render(sprite))
            {
                var observed = Enumerable.Range(0, sprite.Height)
                    .SelectMany(y => Enumerable.Range(0, sprite.Width)
                        .Select(x => renderedSprite.GetPixel(x, y).ToArgb()));

                // Assert.
                observed.Should().Equal(expected);
            }
        }
    }
}
