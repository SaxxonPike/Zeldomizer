using Zeldomizer.Metal;

namespace Zeldomizer.Engine.Text
{
    public class MenuText : IMenuText
    {
        private readonly IStringData _eliminationModeText;
        private readonly IStringData _registerYourNameText;
        private readonly IStringData _registerText;
        private readonly IStringData _specialNameText;

        public MenuText(
            IStringData eliminationModeText, 
            IStringData registerYourNameText, 
            IStringData registerText, 
            IStringData specialNameText)
        {
            _eliminationModeText = eliminationModeText;
            _registerYourNameText = registerYourNameText;
            _registerText = registerText;
            _specialNameText = specialNameText;
        }

        public string EliminationModeText
        {
            get => _eliminationModeText.Text;
            set => _eliminationModeText.Text = value;
        }

        public int EliminationModeTextLength => _eliminationModeText.MaxLength;

        public string RegisterYourNameText
        {
            get => _registerYourNameText.Text;
            set => _registerYourNameText.Text = value;
        }

        public int RegisterYourNameTextLength => _registerYourNameText.MaxLength;

        public string RegisterText
        {
            get => _registerText.Text;
            set => _registerText.Text = value;
        }

        public int RegisterTextLength => _registerText.MaxLength;

        public string SpecialNameText
        {
            get => _specialNameText.Text;
            set => _specialNameText.Text = value;
        }

        public int SpecialNameTextLength => _specialNameText.MaxLength;
    }
}
