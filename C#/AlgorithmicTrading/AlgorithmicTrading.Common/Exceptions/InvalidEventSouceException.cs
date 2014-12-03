using System;

namespace AlgorithmicTrading.Common.Exceptions
{
    public class InvalidEventSouceException : Exception
    {
        public InvalidEventSouceException(string message)
            : base(message)
        {
        }
    }
}