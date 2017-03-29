using System.Collections.Generic;

namespace Zeldomizer.Metal
{
    public interface IPointerTable : IEnumerable<IRom>
    {
        IRom this[int index] { get; }
        int Count { get; }
    }
}