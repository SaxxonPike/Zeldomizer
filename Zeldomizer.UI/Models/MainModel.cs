using Zeldomizer.Randomization.Interfaces;

namespace Zeldomizer.UI.Models
{
    public class MainModel
    {
        public IRandomizer Randomizer { get; }

        public MainModel(IRandomizer randomizer)
        {
            Randomizer = randomizer;
        }
    }
}
