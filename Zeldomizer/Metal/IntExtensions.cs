using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Metal
{
    public static class IntExtensions
    {
        /// <summary>
        /// Determine if a bit is set.
        /// </summary>
        public static bool Bit(this int value, int number)
        {
            return (value & (1 << number)) != 0;
        }

        /// <summary>
        /// Determine if a bit is set.
        /// </summary>
        public static bool Bit(this byte value, int number) =>
            Bit((int) value, number);

        /// <summary>
        /// Get a value which has the specified bit changed.
        /// </summary>
        public static int Bit(this int value, int number, bool newValue)
        {
            var bit = 1 << number;
            return (value & ~bit) | (newValue ? bit : 0);
        }

        /// <summary>
        /// Get a value which has the specified bit changed.
        /// </summary>
        public static byte Bit(this byte value, int number, bool newValue) =>
            unchecked((byte) Bit((int) value, number, newValue));

        /// <summary>
        /// Get a value from the specified range of bits.
        /// </summary>
        public static int Bits(this int value, int number, int downTo)
        {
            return (value & ((2 << number) - 1)) >> downTo;
        }

        /// <summary>
        /// Get a value from the specified range of bits.
        /// </summary>
        public static byte Bits(this byte value, int number, int downTo) =>
            unchecked((byte) Bits((int) value, number, downTo));

        /// <summary>
        /// Get a value which has the specified bits changed.
        /// </summary>
        public static int Bits(this int value, int number, int downTo, int newValue)
        {
            var mask = (((2 << number) - 1) >> downTo) << downTo;
            return (value & ~mask) | ((newValue << downTo) & mask);
        }

        /// <summary>
        /// Get a value which has the specified bits changed.
        /// </summary>
        public static byte Bits(this byte value, int number, int downTo, int newValue)
            => unchecked((byte)Bits((int)value, number, downTo, newValue));
    }
}
