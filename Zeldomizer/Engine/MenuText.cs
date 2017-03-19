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
        private readonly StringData _value1;

        public MenuText(IRom source, IStringConverter stringConverter)
        {
            _value1 = new StringData(source, stringConverter, 0x09D2C, 100);
        }

        public string Value1
        {
            get { return _value1.Text; }
            set { _value1.Text = value; }
        }
    }
}
