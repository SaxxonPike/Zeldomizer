using FluentAssertions;
using NUnit.Framework;
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
        public void BottomText1_ReadsCorrectValue()
        {
            Subject.BottomText1.Should().Be("FINALLY,");

        }
        
        [Test]
        public void BottomText2_ReadsCorrectValue()
        {
            Subject.BottomText2.Should().Be("PEACE RETURNS TO HYRULE.");

        }

        [Test]
        public void BottomText3_ReadsCorrectValue()
        {
            Subject.BottomText3.Should().Be("THIS ENDS THE STORY.");
        }


    }
}
