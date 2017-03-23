using FluentAssertions;
using NUnit.Framework;
using Zeldomizer.Engine.Text;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine
{
    public class EndingTextTests : BaseTestFixture<EndingText>
    {
        protected override EndingText GetTestSubject()
        {
            var conversionTable = new ConversionTable();
            var speechStringConverter = new SpeechStringConverter(conversionTable);
            var textStringConverter = new TextStringConverter(conversionTable);
            var fixedStringConverter = new FixedStringConverter(conversionTable);

            return new EndingText(Rom, speechStringConverter, textStringConverter, fixedStringConverter);
        }

        [Test]
        public void TopText_ReadsCorrectValue()
        {
            Subject.TopText.Should().Be($"THANKS LINK,YOU'RE{NewLine} {NewLine}THE HERO OF HYRULE.");
        }

        [Test]
        public void TopText_WritesCorrectValue()
        {
            var value = Random<string>();
            Subject.TopText = value;
            Subject.TopText.Should().Be(value.ToUpperInvariant());
        }

        [Test]
        public void TopTextLength_HasCorrectValue()
        {
            Subject.TopTextLength.Should().Be(38);
        }

        [Test]
        public void BottomText1_ReadsCorrectValue()
        {
            Subject.BottomText1.Should().Be("FINALLY,");

        }

        [Test]
        public void BottomText1_WritesCorrectValue()
        {
            var value = Random<string>().Substring(0, 5);
            Subject.BottomText1 = value;
            Subject.BottomText1.Should().Be(value.ToUpperInvariant());
        }

        [Test]
        public void BottomText1Length_HasCorrectValue()
        {
            Subject.BottomText1Length.Should().Be(8);
        }

        [Test]
        public void BottomText2_ReadsCorrectValue()
        {
            Subject.BottomText2.Should().Be("PEACE RETURNS TO HYRULE.");

        }

        [Test]
        public void BottomText2_WritesCorrectValue()
        {
            var value = Random<string>().Substring(0, 5);
            Subject.BottomText2 = value;
            Subject.BottomText2.Should().Be(value.ToUpperInvariant());
        }

        [Test]
        public void BottomText2Length_HasCorrectValue()
        {
            Subject.BottomText2Length.Should().Be(24);
        }

        [Test]
        public void BottomText3_ReadsCorrectValue()
        {
            Subject.BottomText3.Should().Be("THIS ENDS THE STORY.");
        }

        [Test]
        public void BottomText3_WritesCorrectValue()
        {
            var value = Random<string>().Substring(0, 5);
            Subject.BottomText3 = value;
            Subject.BottomText3.Should().Be(value.ToUpperInvariant());
        }

        [Test]
        public void BottomText3Length_HasCorrectValue()
        {
            Subject.BottomText3Length.Should().Be(20);
        }
    }
}
