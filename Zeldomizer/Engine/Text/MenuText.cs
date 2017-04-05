using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Text
{
    public class MenuText
    {
        private readonly FixedStringData _eliminationModeText;
        private readonly FixedStringData _registerYourNameText;
        private readonly FixedStringData _registerText;
        private readonly FixedStringData _specialNameText;

        public MenuText(ISource source, IFixedStringConverter stringConverter)
        {
            _eliminationModeText = new FixedStringData(new SourceBlock(source, 0x09D48), stringConverter, 17);
            _registerYourNameText = new FixedStringData(new SourceBlock(source, 0x09D5E), stringConverter, 18);
            _registerText = new FixedStringData(new SourceBlock(source, 0x09D70), stringConverter, 8);
            _specialNameText = new FixedStringData(new SourceBlock(source, 0x09EEB), stringConverter, 5);
        }

        public string EliminationModeText
        {
            get => _eliminationModeText.Text;
            set => _eliminationModeText.Text = value;
        }

        public int EliminationModeTextLength => _eliminationModeText.Length;

        public string RegisterYourNameText
        {
            get => _registerYourNameText.Text;
            set => _registerYourNameText.Text = value;
        }

        public int RegisterYourNameTextLength => _registerYourNameText.Length;

        public string RegisterText
        {
            get => _registerText.Text;
            set => _registerText.Text = value;
        }

        public int RegisterTextLength => _registerText.Length;

        public string SpecialNameText
        {
            get => _specialNameText.Text;
            set => _specialNameText.Text = value;
        }

        public int SpecialNameTextLength => _specialNameText.Length;
    }
}
