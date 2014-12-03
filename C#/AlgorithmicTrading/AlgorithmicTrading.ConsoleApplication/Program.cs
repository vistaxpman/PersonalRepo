using AlgorithmicTrading.Common.Contexts;
using AlgorithmicTrading.Common.EventSources;
using AlgorithmicTrading.Common.PortfolioManagers;
using AlgorithmicTrading.Common.TradingEngines;
using AlgorithmicTrading.Qai;
using AlgorithmicTrading.Strategies;
using System;

namespace AlgorithmicTrading.ConsoleApplication
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IContext context = new HistoricalContext()
                {
                    InstrumentKeys = new[] { "72990", "39988", "46244" },
                    FromTime = DateTime.Parse("2014-01-01"),
                    ToTime = DateTime.Today,
                    Strategy = new WeightedPortfolioStrategy(),
                    InstrumentManager = new QaiInstrumentManager(),
                    PortfolioManager = new WeightedPortfolioPortfolioManager(),
                    TradingEngine = new SuccessTradingEngine(),
                    EventSources =
                        new IEventSource[] { new QaiPriceEventSource(), new QaiStockSplitEventSource(), new HistoricalMaketCloseEventSource(), },
                };

            context.Event += (sender, e) => Console.WriteLine(string.Format("{0}\n", e.Event));
            context.Initialize();
            context.Start();

            Console.ReadLine();
        }
    }
}