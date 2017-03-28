using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Zeldomizer.Engine.Dungeons;

namespace Zeldomizer
{
    public class TestPlayground : BaseTestFixture
    {
        [Test]
        public void Test2()
        {
            var cart = new ZeldaCartridge(Rom);
            var rooms = cart.Dungeons.Rooms.ToArray();
            var compiler = new DungeonRoomCompiler();

            var output = compiler.Compile(rooms);
            //Rom.Write(output.Columns.Select(s => unchecked((byte)s)).ToArray(), 0);
            //var rawColumn = new DungeonColumnLibrary(Rom, 0, 130);
            //var columnOut = rawColumn.ToArray().Select(dc => dc.ToArray());

            //foreach (var column in columnOut)
            //{
            //    foreach (var tile in column)
            //        Console.Write($"{tile:X1}");
            //    Console.WriteLine();
            //}
        }

        private static char GetMapChar(int input)
        {
            var output = '.';
            switch (input)
            {
                case 0:
                    output = 'X';
                    break;
                case 1:
                    output = '.';
                    break;
                case 2:
                case 3:
                    output = '&';
                    break;
                case 4:
                    output = 'v';
                    break;
                case 5:
                    output = ',';
                    break;
                case 6:
                    output = '~';
                    break;
                case 7:
                    output = ' ';
                    break;
            }
            return output;
        }

        private void OutputMap(IReadOnlyList<int> data)
        {
            var i = 0;
            for (var y = 0; y < 7; y++)
            {
                for (var x = 0; x < 12; x++)
                {
                    Console.Write(GetMapChar(data[i++]));
                }
                Console.WriteLine();
            }
        }

        [Test]
        public void Test1()
        {
            var cart = new ZeldaCartridge(Rom);
            var rooms = cart.Dungeons.Rooms.ToArray();
            var roomNr = 0;

            foreach (var room in rooms)
            {
                Console.WriteLine($"Room {roomNr:X2}");
                roomNr++;
                var data = room.ToArray();
                OutputMap(data);
            }
        }

    }
}
