using Zeldomizer.Metal;

namespace Zeldomizer.Engine
{
    public class CharacterText : StringList 
    {
        private readonly IStringFormatter _stringFormatter;

        public CharacterText(IRom source, IStringFormatter stringFormatter, IStringConverter stringConverter)
            : base(source, stringConverter, 0x04000, -0x04000, 0x556, 0x26)
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
