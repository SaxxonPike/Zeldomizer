using System.Collections.Generic;
using Zeldomizer.Engine.Underworld.Interfaces;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.AI
{
    public class UnderworldMapper
    {
        private readonly IUnderworld _underworld;

        public UnderworldMapper(IUnderworld underworld)
        {
            _underworld = underworld;
        }
    }
}
