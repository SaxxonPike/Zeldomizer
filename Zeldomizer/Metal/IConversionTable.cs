using System.Collections.Generic;

namespace Zeldomizer.Metal
{
    public interface IConversionTable
    {
        int? Encode(char input);
        char? Decode(int input);
    }
}