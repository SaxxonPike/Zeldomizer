using FluentAssertions;
using NUnit.Framework;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine
{
    public class MenuTextTests : BaseTestFixture<MenuText>
    {
        protected override MenuText GetTestSubject()
        {
            var conversionTable = new ConversionTable();
            var stringConverter = new FixedStringConverter(conversionTable);
            return new MenuText(Rom, stringConverter);
        }

        [Test]
        public void EliminationModeText_ReadsCorrectValue()
        {
            Subject.EliminationModeText.Should().Be("ELIMINATION  MODE");
        }

        [Test]
        public void EliminationModeText_WritesCorrectValue()
        {
            var value = Random<string>().Substring(0, 5);
            Subject.EliminationModeText = value;
            Subject.EliminationModeText.Should().Be(value.ToUpperInvariant());
        }

        [Test]
        public void EliminationModeTextLength_HasCorrectValue()
        {
            Subject.EliminationModeTextLength.Should().Be(17);
        }

        [Test]
        public void RegisterYourNameText_ReadsCorrectValue()
        {
            Subject.RegisterYourNameText.Should().Be("REGISTER YOUR NAME");
        }

        [Test]
        public void RegisterYourNameText_WritesCorrectValue()
        {
            var value = Random<string>().Substring(0, 5);
            Subject.RegisterYourNameText = value;
            Subject.RegisterYourNameText.Should().Be(value.ToUpperInvariant());
        }

        [Test]
        public void RegisterYourNameTextLength_HasCorrectValue()
        {
            Subject.RegisterYourNameTextLength.Should().Be(18);
        }

    }
}
