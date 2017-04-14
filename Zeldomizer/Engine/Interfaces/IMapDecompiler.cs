using System.Collections.Generic;

namespace Zeldomizer.Engine.Interfaces
{
    public interface IMapDecompiler
    {
        IDecompiledMap Decompile(IEnumerable<IEnumerable<IEnumerable<int>>> columnLibraryList, IEnumerable<IEnumerable<int>> roomList);
    }
}