using System;
using System.Linq;
using NUnit.Framework;

namespace Zeldomizer
{
    public class TestPlayground : BaseTestFixture
    {
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
                var i = 0;
                for (var y = 0; y < 7; y++)
                {
                    for (var x = 0; x < 12; x++)
                    {
                        char output = '.';
                        switch (data[i++])
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
                        Console.Write(output);
                    }
                    Console.WriteLine();
                }
            }
        }

    }
}
