namespace Zeldomizer.Metal
{
    public class NullStringFormatter : IStringFormatter
    {
        public string Format(string input)
        {
            return input;
        }

        public string UnFormat(string input)
        {
            return input;
        }
    }
}
