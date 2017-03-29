using FluentAssertions;
using NUnit.Framework;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Text
{
    public class MenuTextTests : BaseTestFixture<MenuText>
    {
        protected override MenuText GetTestSubject()
        {
            var conversionTable = new TextConversionTable();
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

        [Test]
        public void RegisterText_ReadsCorrectValue()
        {
            Subject.RegisterText.Should().Be("REGISTER");
        }

        [Test]
        public void RegisterText_WritesCorrectValue()
        {
            var value = Random<string>().Substring(0, 5);
            Subject.RegisterText = value;
            Subject.RegisterText.Should().Be(value.ToUpperInvariant());
        }

        [Test]
        public void RegisterTextLength_HasCorrectValue()
        {
            Subject.RegisterTextLength.Should().Be(8);
        }

        [Test]
        public void SpecialNameText_ReadsCorrectValue()
        {
            Subject.SpecialNameText.Should().Be("ZELDA");
        }

        [Test]
        public void SpecialNameText_WritesCorrectValue()
        {
            var value = Random<string>().Substring(0, 4);
            Subject.SpecialNameText = value;
            Subject.SpecialNameText.Should().Be(value.ToUpperInvariant());
        }

        [Test]
        public void SpecialNameTextLength_HasCorrectValue()
        {
            Subject.SpecialNameTextLength.Should().Be(5);
        }
    }
}
