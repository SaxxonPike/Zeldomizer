using System;

namespace Zeldomizer.Randomization
{
    public class RandomizerModuleValidationException : Exception
    {
        public RandomizerModuleValidationException(string message) : base(message)
        {
        }

        public RandomizerModuleValidationException(string message, Exception innerException) : base(message,
            innerException)
        {
        }
    }
}
