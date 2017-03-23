using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Text
{
    public class EndingText
    {
        private readonly IRom _source;
        private readonly StringData _topText;
        private readonly FixedStringData _bottomText1;
        private readonly FixedStringData _bottomText2;
        private readonly FixedStringData _bottomText3;


        public EndingText(IRom source, IStringConverter speechConverter, IStringConverter textConverter, IFixedStringConverter fixedStringConverter)
        {
            _source = source;
            _topText = new StringData(source, speechConverter, 0xA959, 38);
            _bottomText1 = new FixedStringData(source, fixedStringConverter, 0xAB07, 8);
            _bottomText2 = new FixedStringData(source, fixedStringConverter, 0xAB0F, 24);
            _bottomText3 = new FixedStringData(source, fixedStringConverter, 0xAB27, 20);
        }

        public string TopText
        {
            get { return _topText.Text; }
            set { _topText.Text = value; }
        }

        public int TopTextLength => _topText.MaxLength;

        public string BottomText1
        {
            get { return _bottomText1.Text; }
            set { _bottomText1.Text = value; }
        }

        public int BottomText1Length => _bottomText1.Length;

        public string BottomText2
        {
            get { return _bottomText2.Text; }
            set { _bottomText2.Text = value; }
        }

        public int BottomText2Length => _bottomText2.Length;

        public string BottomText3
        {
            get { return _bottomText3.Text; }
            set { _bottomText3.Text = value; }
        }

        public int BottomText3Length => _bottomText3.Length;
    }
}
