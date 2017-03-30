using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Overworld
{
    public class OverworldColumn : IEnumerable<int>
    {
        private readonly ISource _source;

        public OverworldColumn(ISource source)
        {
            _source = source;
        }

        private IEnumerable<int> GetTiles()
        {
            var i = 0;
            while (i < 11)
            {
                yield return _source[i] & 0x7F;
                i++;
            }
        }

        public override string ToString()
        {
            return string.Join(" ", this.Select(i => $"{i:x2}"));
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public IEnumerator<int> GetEnumerator() => GetTiles().GetEnumerator();
    }
}
