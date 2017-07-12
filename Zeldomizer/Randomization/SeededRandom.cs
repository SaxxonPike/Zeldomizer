using System;
using Zeldomizer.Randomization.Interfaces;

namespace Zeldomizer.Randomization
{
    public class SeededRandom : ISeededRandom
    {
        private Random _random;

        public SeededRandom(int seed)
        {
            Seed = seed;
            Reset();
        }

        public int Seed { get; }
        public void Reset() => _random = new Random(Seed);
        public int GetInt(int max) => _random.Next(max);
        public int GetInt(int min, int max) => _random.Next(min, max + 1);
        public bool GetBool() => _random.Next(2) == 0;
    }
}
