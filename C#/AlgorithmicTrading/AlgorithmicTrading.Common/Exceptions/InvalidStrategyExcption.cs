using System;

namespace AlgorithmicTrading.Common.Exceptions
{
    public class InvalidStrategyExcption : Exception
    {
        public InvalidStrategyExcption(string message)
            : base(message)
        {
        }
    }
}