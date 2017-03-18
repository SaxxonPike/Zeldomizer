using Zeldomizer.Metal;

namespace Zeldomizer.Engine
{
    public class CharacterText : StringList 
    {
        public CharacterText(byte[] source) : base(source, 0x04000, -0x04000, 0x555, 0x26)
        {
        }

        protected override string Decode(int index)
        {
            return base.Decode(index);
        }

        protected override byte[] Encode(string text)
        {
            return base.Encode(text);
        }
    }
}
