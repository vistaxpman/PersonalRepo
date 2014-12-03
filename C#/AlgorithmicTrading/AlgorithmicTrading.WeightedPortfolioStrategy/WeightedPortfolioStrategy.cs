using AlgorithmicTrading.Common.EventEngines;
using AlgorithmicTrading.Common.Events;
using AlgorithmicTrading.Common.Strategies;
using System;
using System.Linq;

namespace AlgorithmicTrading.Strategies
{
    public class WeightedPortfolioStrategy : Strategy
    {
        private readonly IEventEngine _eventEngine = new WeightedPortfolioEventEngine();
        private WeightedPortfolioThreshold[] _thresholds;

        public override Type[] InputEventTypes
        {
            get { return new[] { typeof(StockSplitEvent), typeof(PriceEvent), typeof(DailyMarketCloseEvent), typeof(TradeEvent), }; }
        }

        public override Type[] OutputEventTypes
        {
            get { return new Type[0]; }
        }

        public override IEventEngine EventEngine
        {
            get { return _eventEngine; }
        }

        public override void Initialize()
        {
            base.Initialize();
            _thresholds = new[] { new WeightedPortfolioThreshold { InstrumentKey = "72990", LowerLimit = .73, UpperLimit = .74 }, };
        }

        public override void ProcessFeedbackEvent(FeedbackEvent @event)
        {
            switch (@event.Feedback)
            {
                case "PriceChanged":
                    NewPricesArrived((ClosePricesEvent)@event.Payload);
                    break;

                case "StockSplit":
                    StockSplitArrived((StockSplitEvent)@event.Payload);
                    break;

                case "Trade":
                    NewTradeArraived((TradeEvent)@event.Payload);
                    break;

                default:
                    throw new ArgumentException("Unknown feedback type.");
            }
        }

        protected override void PreValidate()
        {
        }

        protected override void PostValidate()
        {
        }

        private void NewPricesArrived(ClosePricesEvent closePricesEvent)
        {
            var prices = closePricesEvent.Prices;
            var positions = Context.PortfolioManager.GetPosition();

            var marketValues = (from position in positions
                                let price = prices[position.Key]
                                select new { InstrumentKey = position.Key, Value = position.Value * price, Price = price }).ToArray();

            var total = marketValues.Sum(v => v.Value);
#if DEBUG
            Console.WriteLine("Strategy");
            foreach (var marketValue in marketValues)
            {
                Console.WriteLine("[{0}] Ratio: {1:P2}", marketValue.InstrumentKey, marketValue.Value / total);
            }
#endif

            var exceptions = (from threshold in _thresholds
                              join marketValue in marketValues on threshold.InstrumentKey equals marketValue.InstrumentKey
                              let ratio = marketValue.Value / total
                              where ratio < threshold.LowerLimit || ratio > threshold.UpperLimit
                              select new { Threshold = threshold, MarketValue = marketValue, Ratio = ratio, Direction = ratio < threshold.LowerLimit ? "Lower" : "Upper" }).ToArray();

#if DEBUG
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var exception in exceptions)
            {
                Console.WriteLine("[{0}] Ratio: {3:P2} Threshold: {1:P2}/{2:P2} ",
                    exception.MarketValue.InstrumentKey, exception.Threshold.LowerLimit, exception.Threshold.UpperLimit, exception.Ratio);
            }
            Console.ResetColor();
#endif

            var orders = (from exception in exceptions
                          let instrumentKey = exception.MarketValue.InstrumentKey
                          let threshold = exception.Threshold
                          let delta = exception.Direction == "Lower" ? threshold.LowerLimit - exception.Ratio : threshold.UpperLimit - exception.Ratio
                          select new OrderEvent
                          {
                              EventTime = EventEngine.CurrenTime,
                              InstrumentKey = instrumentKey,
                              Quantity = delta * positions[instrumentKey],
                              Price = prices[instrumentKey]
                          }).ToArray();

#if DEBUG
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var order in orders)
            {
                Console.WriteLine(@order);
            }
            Console.ResetColor();
            Console.WriteLine();
#endif
            if (orders.Any())
            {
                Context.TradingEngine.ExcuteOrders(orders); 
            }
        }

        private void StockSplitArrived(StockSplitEvent stockSplitEvent)
        {
            var portfolioManager = Context.PortfolioManager;
            var positions = portfolioManager.GetPosition();
            double quantity;

            if (positions.TryGetValue(stockSplitEvent.InstrumentKey, out quantity))
            {
                var newQuantity = quantity * (stockSplitEvent.NewShares / stockSplitEvent.OldShares - 1);
                var trade = new TradeEvent()
                    {
                        InstrumentKey = stockSplitEvent.InstrumentKey,
                        Quantity = newQuantity,
                        EventTime = stockSplitEvent.EventTime,
                        Comment = "StockSplit"
                    };
#if DEBUG
                Console.WriteLine("Strategy");
                Console.WriteLine(trade);
#endif
                portfolioManager.ChangePosition(trade);

#if DEBUG
                foreach (var position in portfolioManager.GetPosition())
                {
                    Console.WriteLine("[{0}] Quantity: {1:F}", position.Key, position.Value);
                }
                Console.WriteLine();
#endif
            }
        }

        private void NewTradeArraived(TradeEvent tradeEvent)
        {
            var portfolioManager = Context.PortfolioManager;
            portfolioManager.ChangePosition(tradeEvent);
#if DEBUG
            Console.WriteLine("Strategy");
            Console.WriteLine(tradeEvent);
            foreach (var position in portfolioManager.GetPosition())
            {
                Console.WriteLine("[{0}] Quantity: {1:F}", position.Key, position.Value);
            }
            Console.WriteLine();
#endif
        }
    }
}