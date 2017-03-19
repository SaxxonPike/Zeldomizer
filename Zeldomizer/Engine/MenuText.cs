using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeldomizer.Metal;

namespace Zeldomizer.Engine
{
    public class MenuText
    {
        private readonly FixedStringData _eliminationModeText;
        private readonly FixedStringData _registerYourNameText;
        private readonly FixedStringData _registerText;
        private readonly FixedStringData _specialNameText;

        public MenuText(IRom source, IFixedStringConverter stringConverter)
        {
            _eliminationModeText = new FixedStringData(source, stringConverter, 0x09D48, 17);
            _registerYourNameText = new FixedStringData(source, stringConverter, 0x09D5E, 18);
            _registerText = new FixedStringData(source, stringConverter, 0x09D70, 8);
            _specialNameText = new FixedStringData(source, stringConverter, 0x09EEB, 5);
        }

        public string EliminationModeText
        {
            get { return _eliminationModeText.Text; }
            set { _eliminationModeText.Text = value; }
        }

        public int EliminationModeTextLength => _eliminationModeText.Length;

        public string RegisterYourNameText
        {
            get { return _registerYourNameText.Text; }
            set { _registerYourNameText.Text = value; }
        }

        public int RegisterYourNameTextLength => _registerYourNameText.Length;

        public string RegisterText
        {
            get { return _registerText.Text; }
            set { _registerText.Text = value; }
        }

        public int RegisterTextLength => _registerText.Length;

        public string SpecialNameText
        {
            get { return _specialNameText.Text; }
            set { _specialNameText.Text = value; }
        }

        public int SpecialNameTextLength => _specialNameText.Length;
    }
}
