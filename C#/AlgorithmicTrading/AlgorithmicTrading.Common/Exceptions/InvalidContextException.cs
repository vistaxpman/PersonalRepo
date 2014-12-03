using System;

namespace AlgorithmicTrading.Common.Exceptions
{
    public class InvalidContextException : ApplicationException
    {
        public InvalidContextException(string message)
            : base(message)
        {
        }
    }
}