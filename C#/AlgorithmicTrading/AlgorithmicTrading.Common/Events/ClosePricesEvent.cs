using System.Collections.Generic;
using System.Text;

namespace AlgorithmicTrading.Common.Events
{
    public class ClosePricesEvent : BusinessEvent
    {
        public Dictionary<string, double> Prices { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(base.ToString());
            foreach (var price in Prices)
            {
                builder.AppendFormat("\n[{0}] Price: {1:F}", price.Key, price.Value);
            }

            return builder.ToString();
        }
    }
}