using System;
using NUnit.Framework;
using Zeldomizer.Engine;

namespace Zeldomizer.Test.Engine
{
    public class CharacterTextTests : BaseTestFixture<CharacterText>
    {
        protected override CharacterText GetTestSubject()
        {
            return new CharacterText(Rom);
        }

        [Test]
        public void Test1()
        {
            foreach (var text in Subject)
            {
                Console.WriteLine(text);
            }
        }
    }
}
