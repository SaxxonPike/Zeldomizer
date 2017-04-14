using System.Collections.Generic;

namespace Zeldomizer.Engine.Interfaces
{
    public interface IMapCompiler
    {
        ICompiledMap Compile(IEnumerable<IEnumerable<int>> data);
    }
}