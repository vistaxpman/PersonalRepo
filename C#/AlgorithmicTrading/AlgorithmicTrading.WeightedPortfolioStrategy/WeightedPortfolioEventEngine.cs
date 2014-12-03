using AlgorithmicTrading.Common.Events;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmicTrading.Common.EventEngines
{
    public class WeightedPortfolioEventEngine : EsperEventEngine
    {
        public override void Initialize()
        {
            base.Initialize();
            Administrator.CreateEPL("create window PriceWindow.std:groupwin(InstrumentKey).std:lastevent() as PriceEvent");
            Administrator.CreateEPL("insert into PriceWindow select * from PriceEvent");
            var statement = Administrator.CreateEPL("on DailyMarketCloseEvent as MarketCloseEvent select MarketCloseEvent.EventTime as EventTime, Price.InstrumentKey as InstrumentKey, Price.Price as Price from PriceWindow as Price");
            statement.Events += (sender, e) =>
                {
                    var prices = (from @event in e.NewEvents
                                  select new KeyValuePair<string, double>((string)@event["InstrumentKey"], (double)@event["Price"])).ToDictionary(p => p.Key, p => p.Value);
                    var closePricesEvent = new ClosePricesEvent { Prices = prices };
                    var feedbackEvent = new FeedbackEvent("PriceChanged", closePricesEvent);
                    OnFeedback(new EventEventArgs(feedbackEvent));
                };

            statement = Administrator.CreateEPL("select * from StockSplitEvent");
            statement.Events += (sender, e) =>
            {
                var stockSplitEvent = (StockSplitEvent)e.NewEvents[0].Underlying;
                var feedbackEvent = new FeedbackEvent("StockSplit", stockSplitEvent);
                OnFeedback(new EventEventArgs(feedbackEvent));
            };

            statement = Administrator.CreateEPL("select * from TradeEvent");
            statement.Events += (sender, e) =>
            {
                var tradeEvent = (TradeEvent)e.NewEvents[0].Underlying;
                var feedbackEvent = new FeedbackEvent("Trade", tradeEvent);
                OnFeedback(new EventEventArgs(feedbackEvent));
            };
        }
    }
}