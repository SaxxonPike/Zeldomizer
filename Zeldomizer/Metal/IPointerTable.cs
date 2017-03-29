using System.Collections.Generic;

namespace Zeldomizer.Metal
{
    public interface IPointerTable : IEnumerable<ISource>
    {
        ISource Source { get; }
        ISource this[int index] { get; }
        int Count { get; }
        int GetPointer(int index);
        void SetPointer(int index, int value);
    }
}