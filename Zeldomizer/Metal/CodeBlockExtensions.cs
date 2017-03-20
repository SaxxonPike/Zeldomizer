﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeldomizer.Metal
{
    public static class CodeBlockExtensions
    {
        public static int ConvertRomOffsetToMappedAddress(this ICodeBlock codeBlock, int offset)
        {
            return offset - codeBlock.Offset + codeBlock.Origin;
        }

        public static int ConvertMappedAddressToRomOffset(this ICodeBlock codeBlock, int address)
        {
            return address - codeBlock.Origin + codeBlock.Offset;
        }
    }
}
