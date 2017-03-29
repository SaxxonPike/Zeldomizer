using System.Collections.Generic;

namespace Zeldomizer.Metal
{
    public interface IPointerTable : IEnumerable<ISource>
    {
        ISource this[int index] { get; }
        int Count { get; }
    }
}