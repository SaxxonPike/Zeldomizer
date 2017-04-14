using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Text
{
    public class EndingText : IEndingText
    {
        private readonly IStringData _topText;
        private readonly IStringData _bottomText1;
        private readonly IStringData _bottomText2;
        private readonly IStringData _bottomText3;


        public EndingText(
            IStringData topText,
            IStringData bottomText1,
            IStringData bottomText2,
            IStringData bottomText3)
        {
            _topText = topText;
            _bottomText1 = bottomText1;
            _bottomText2 = bottomText2;
            _bottomText3 = bottomText3;
        }

        public string TopText
        {
            get => _topText.Text;
            set => _topText.Text = value;
        }

        public int TopTextLength => _topText.MaxLength;

        public string BottomText1
        {
            get => _bottomText1.Text;
            set => _bottomText1.Text = value;
        }

        public int BottomText1Length => _bottomText1.MaxLength;

        public string BottomText2
        {
            get => _bottomText2.Text;
            set => _bottomText2.Text = value;
        }

        public int BottomText2Length => _bottomText2.MaxLength;

        public string BottomText3
        {
            get => _bottomText3.Text;
            set => _bottomText3.Text = value;
        }

        public int BottomText3Length => _bottomText3.MaxLength;
    }
}
