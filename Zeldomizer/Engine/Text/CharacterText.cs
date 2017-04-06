using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Text
{
    public class CharacterText : StringList 
    {
        private readonly IStringFormatter _stringFormatter;

        public CharacterText(IPointerTable pointerTable, IStringFormatter stringFormatter, IStringConverter stringConverter)
            : base(pointerTable, stringConverter, 0x556, 0x26)
        {
            _stringFormatter = stringFormatter;
        }

        protected override string Decode(int index)
        {
            return _stringFormatter.UnFormat(base.Decode(index));
        }

        protected override byte[] Encode(string text)
        {
            return base.Encode(_stringFormatter.Format(text));
        }
    }
}
