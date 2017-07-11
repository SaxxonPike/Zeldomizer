using System.Collections.Generic;

namespace Zeldomizer.Randomization
{
    public interface IRandomizerModule
    {
        bool Enabled { get; set; }
        string Name { get; }
        string Description { get; }
        IReadOnlyList<IRandomizerParameter> Parameters { get; }

        void Activate(ZeldaCartridge cartridge);
    }
}
