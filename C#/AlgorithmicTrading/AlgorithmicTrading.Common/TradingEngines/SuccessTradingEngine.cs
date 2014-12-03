using AlgorithmicTrading.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmicTrading.Common.TradingEngines
{
    public class SuccessTradingEngine : TradingEngine
    {
        public SuccessTradingEngine()
            : base(RunMode.History)
        {
        }

        public override void Initialize()
        {
        }

        public override void Start()
        {
        }

        public override void ExcuteOrders(IEnumerable<OrderEvent> orders)
        {
            var trades = (from order in orders
                          select new TradeEvent
                              {
                                  EventTime = order.EventTime,
                                  InstrumentKey = order.InstrumentKey,
                                  Quantity = order.Quantity,
                                  Price = order.Price.Value,
                                  Comment = "Trade",
                              }).ToArray();
#if DEBUG
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("TradingEngine");
            foreach (var trade in trades)
            {
                Console.WriteLine(trade);
            }
            Console.WriteLine();
            Console.ResetColor();
#endif

            foreach (var trade in trades)
            {
                OnTradeTradeSuccessed(new EventEventArgs(trade));
            }
        }

        protected override void PreValidate()
        {
        }

        protected override void PostValidate()
        {
        }
    }
}