using System.Linq;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Scenes
{
    public class SceneRow : ByteList
    {
        private readonly ISource _source;
        private readonly ITextConversionTable _textConversionTable;

        public SceneRow(ISource source, ITextConversionTable textConversionTable) : base(source, 32)
        {
            _source = source;
            _textConversionTable = textConversionTable;
        }

        public void Write(int x, string text)
        {
            var ix = x;

            foreach (var b in text.ToUpper().Select(_textConversionTable.Encode))
            {
                if (ix < 32)
                    _source[ix++] = unchecked((byte)(b ?? _textConversionTable.SpaceCharacter));
            }
        }

        public override string ToString()
        {
            return new string(this.Select(b => _textConversionTable.Decode(b) ?? ' ').ToArray());
        }
    }
}
