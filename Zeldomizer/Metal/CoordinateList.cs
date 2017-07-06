using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Metal
{
    public class CoordinateList : FixedList<ICoordinate>
    {
        private readonly ISource _source;
        private readonly int _width;
        private readonly int _height;

        public CoordinateList(ISource source, int capacity, int width, int height) : base(capacity)
        {
            _source = source;
            _width = width;
            _height = height;
        }

        public override ICoordinate this[int index]
        {
            get => new Coordinate(new SourceBlock(_source, index), _width, _height);
            set => throw new Exception("Can't set items in a coordinate list (yet...)");
        }
    }
}
