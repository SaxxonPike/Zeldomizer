using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace Zeldomizer.Engine.Dungeons
{
    public class DungeonColumnMacroLibraryListTests : BaseTestFixture<DungeonColumnLibraryList>
    {
        protected override DungeonColumnLibraryList GetTestSubject()
        {
            return new DungeonColumnLibraryList(Rom, 0x16704, 0xC000, 10);
        }

        [Test]
        [TestCase(0x00, "1111111")]
        [TestCase(0x01, "0111110")]
        [TestCase(0x02, "1111101")]
        [TestCase(0x03, "0004000")]
        [TestCase(0x04, "0001000")]
        [TestCase(0x05, "0001111")]
        [TestCase(0x06, "0011100")]
        [TestCase(0x07, "0011101")]
        [TestCase(0x10, "1014101")]
        [TestCase(0x11, "1010101")]
        [TestCase(0x12, "1011101")]
        [TestCase(0x13, "1000001")]
        [TestCase(0x14, "1000101")]
        [TestCase(0x15, "1040101")]
        [TestCase(0x16, "0011111")]
        [TestCase(0x17, "0111111")]
        [TestCase(0x20, "1111110")]
        [TestCase(0x21, "0111011")]
        [TestCase(0x22, "1100011")]
        [TestCase(0x23, "1101011")]
        [TestCase(0x24, "0000001")]
        [TestCase(0x25, "1011111")]
        [TestCase(0x26, "1111011")]
        [TestCase(0x30, "1101110")]
        [TestCase(0x31, "1110111")]
        [TestCase(0x32, "1111611")]
        [TestCase(0x33, "1101111")]
        [TestCase(0x34, "1111010")]
        [TestCase(0x35, "1011100")]
        [TestCase(0x36, "0077211")]
        [TestCase(0x40, "1177177")]
        [TestCase(0x41, "1661111")]
        [TestCase(0x42, "1111711")]
        [TestCase(0x43, "1161111")]
        [TestCase(0x44, "7111177")]
        [TestCase(0x45, "1013101")]
        [TestCase(0x46, "1012101")]
        [TestCase(0x50, "0100113")]
        [TestCase(0x51, "3777773")]
        [TestCase(0x52, "3111113")]
        [TestCase(0x53, "1111113")]
        [TestCase(0x54, "0010011")]
        [TestCase(0x55, "1101100")]
        [TestCase(0x56, "0077311")]
        [TestCase(0x57, "0000011")]
        [TestCase(0x60, "0000000")]
        [TestCase(0x61, "6666666")]
        [TestCase(0x62, "1666661")]
        [TestCase(0x63, "6666616")]
        [TestCase(0x64, "1616161")]
        [TestCase(0x65, "6161616")]
        [TestCase(0x66, "1611161")]
        [TestCase(0x67, "6166616")]
        [TestCase(0x70, "6111116")]
        [TestCase(0x71, "6166666")]
        [TestCase(0x72, "1111112")]
        [TestCase(0x73, "6661666")]
        [TestCase(0x74, "1666666")]
        [TestCase(0x75, "1661166")]
        [TestCase(0x76, "7777777")]
        [TestCase(0x77, "5555555")]
        [TestCase(0x80, "1001001")]
        [TestCase(0x81, "1777111")]
        [TestCase(0x82, "1116111")]
        [TestCase(0x83, "0100112")]
        [TestCase(0x84, "2777772")]
        [TestCase(0x85, "2111112")]
        [TestCase(0x86, "0100111")]
        [TestCase(0x87, "1112111")]
        [TestCase(0x88, "1113111")]
        [TestCase(0x90, "0777111")]
        [TestCase(0x91, "1021101")]
        [TestCase(0x92, "1031101")]
        [TestCase(0x93, "1212121")]
        [TestCase(0x94, "1212125")]
        [TestCase(0x95, "1111155")]
        [TestCase(0x96, "1111555")]
        [TestCase(0x97, "1313131")]
        [TestCase(0x98, "1313135")]
        public void List_ShouldDecompressColumns(int index, string expected)
        {
            var tiles = Subject[index >> 4][index & 0xF].ToArray();
            var tileIds = string.Join("", tiles.Select(c => $"{c:X1}"));

            Console.WriteLine($"Input:     ${index:X2}");
            Console.WriteLine($"Expected:  {expected}");
            Console.WriteLine($"Observed:  {tileIds}");
            tileIds.Should().Be(expected);
        }
    }
}
