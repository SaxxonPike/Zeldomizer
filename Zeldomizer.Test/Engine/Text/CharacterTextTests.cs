using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Text
{
    public class CharacterTextTests : BaseTestFixture<CharacterText>
    {
        protected override CharacterText GetTestSubject()
        {
            var stringFormatter = new StringFormatter();
            var textConversionTable = new TextConversionTable();
            var stringConverter = new SpeechStringConverter(textConversionTable);
            return new CharacterText(new WordPointerTable(new SourceBlock(Source, 0x4000), new SourceBlock(Source, -0x4000), 0x26), stringFormatter, stringConverter);
        }

        [Test]
        public void CharacterText_CanWriteText()
        {
            var randomString = Random<string>();

            var expected = new[]
            {
                string.Empty,
                randomString.ToUpperInvariant(),
                "TAKE ANY ROAD YOU WANT.",
                "SECRET IS IN THE TREE AT THE DEAD-END.",
                "LET'S PLAY MONEY MAKING GAME.",
                "PAY ME FOR THE DOOR REPAIR CHARGE.",
                "SHOW THIS TO THE OLD WOMAN.",
                "MEET THE OLD MAN AT THE GRAVE.",
                "BUY MEDICINE BEFORE YOU GO.",
                "PAY ME AND I'LL TALK.",
                "THIS AIN'T ENOUGH TO TALK.",
                "GO UP,UP, THE MOUNTAIN AHEAD.",
                "GO NORTH,WEST,SOUTH, WEST TO THE FOREST OF MAZE.",
                "BOY, YOU'RE RICH!",
                "BUY SOMETHIN' WILL YA!",
                "BOY, THIS IS REALLY EXPENSIVE!",
                "TAKE ANY ONE YOU WANT.",
                "IT'S A SECRET TO EVERYBODY.",
                "GRUMBLE,GRUMBLE...",
                "EASTMOST PENNINSULA IS THE SECRET.",
                "DODONGO DISLIKES SMOKE.",
                "DID YOU GET THE SWORD FROM THE OLD MAN ON TOP OF THE WATERFALL?",
                "WALK INTO THE WATERFALL.",
                "SECRET POWER IS SAID TO BE IN THE ARROW.",
                "DIGDOGGER HATES CERTAIN KIND OF SOUND.",
                "I BET YOU'D LIKE TO HAVE MORE BOMBS.",
                "IF YOU GO IN THE DIRECTION OF THE ARROW.",
                "LEAVE YOUR LIFE OR MONEY.",
                "THERE ARE SECRETS WHERE FAIRIES DON'T LIVE.",
                "AIM AT THE EYES OF GOHMA.",
                "SOUTH OF ARROW MARK HIDES A SECRET.",
                "THERE'S A SECRET IN THE TIP OF THE NOSE.",
                "SPECTACLE ROCK IS AN ENTRANCE TO DEATH.",
                "10TH ENEMY HAS THE BOMB.",
                "ONES WHO DOES NOT HAVE TRIFORCE CAN'T GO IN.",
                "PATRA HAS THE MAP.",
                "GO TO THE NEXT ROOM.",
                "EYES OF SKULL HAS A SECRET."
            };

            Subject[0] = string.Empty;
            Subject[1] = randomString;

            Subject.ToArray().ShouldAllBeEquivalentTo(expected);
        }

        [Test]
        public void CharacterText_CanReadText()
        {
            var expected = new[]
            {
                "IT'S DANGEROUS TO GO ALONE! TAKE THIS.",
                "MASTER USING IT AND YOU CAN HAVE THIS.",
                "TAKE ANY ROAD YOU WANT.",
                "SECRET IS IN THE TREE AT THE DEAD-END.",
                "LET'S PLAY MONEY MAKING GAME.",
                "PAY ME FOR THE DOOR REPAIR CHARGE.",
                "SHOW THIS TO THE OLD WOMAN.",
                "MEET THE OLD MAN AT THE GRAVE.",
                "BUY MEDICINE BEFORE YOU GO.",
                "PAY ME AND I'LL TALK.",
                "THIS AIN'T ENOUGH TO TALK.",
                "GO UP,UP, THE MOUNTAIN AHEAD.",
                "GO NORTH,WEST,SOUTH, WEST TO THE FOREST OF MAZE.",
                "BOY, YOU'RE RICH!",
                "BUY SOMETHIN' WILL YA!",
                "BOY, THIS IS REALLY EXPENSIVE!",
                "TAKE ANY ONE YOU WANT.",
                "IT'S A SECRET TO EVERYBODY.",
                "GRUMBLE,GRUMBLE...",
                "EASTMOST PENNINSULA IS THE SECRET.",
                "DODONGO DISLIKES SMOKE.",
                "DID YOU GET THE SWORD FROM THE OLD MAN ON TOP OF THE WATERFALL?",
                "WALK INTO THE WATERFALL.",
                "SECRET POWER IS SAID TO BE IN THE ARROW.",
                "DIGDOGGER HATES CERTAIN KIND OF SOUND.",
                "I BET YOU'D LIKE TO HAVE MORE BOMBS.",
                "IF YOU GO IN THE DIRECTION OF THE ARROW.",
                "LEAVE YOUR LIFE OR MONEY.",
                "THERE ARE SECRETS WHERE FAIRIES DON'T LIVE.",
                "AIM AT THE EYES OF GOHMA.",
                "SOUTH OF ARROW MARK HIDES A SECRET.",
                "THERE'S A SECRET IN THE TIP OF THE NOSE.",
                "SPECTACLE ROCK IS AN ENTRANCE TO DEATH.",
                "10TH ENEMY HAS THE BOMB.",
                "ONES WHO DOES NOT HAVE TRIFORCE CAN'T GO IN.",
                "PATRA HAS THE MAP.",
                "GO TO THE NEXT ROOM.",
                "EYES OF SKULL HAS A SECRET."
            };

            Subject.ToArray().ShouldAllBeEquivalentTo(expected);
        }
    }
}
