using com.espertech.esper.client;
using System;

namespace CepSamples
{
    public class Apple
    {
        public int Id { get; set; }
        public int Price { get; set; }
    }

    public class Test
    {
        public static void Main(String[] args)
        {
            var provider = EPServiceProviderManager.GetDefaultProvider();

            var admin = provider.EPAdministrator;

            const string epl = "select avg(Price) from CepSamples.Apple.win:length(3)";

            var statement = admin.CreateEPL(epl);
            statement.Events += (sender, e) =>
                {
                    var avg = (double)e.NewEvents[0].Get("avg(Price)");
                    Console.WriteLine("The average price is {0}.", avg);
                };

            var runtime = provider.EPRuntime;

            runtime.SendEvent(new Apple {Id = 1, Price = 5});

            runtime.SendEvent(new Apple { Id = 2, Price = 10 });

            runtime.SendEvent(new Apple { Id = 3, Price = 15 });

            runtime.SendEvent(new Apple { Id = 3, Price = 20 });
            
            Console.ReadLine();
        }
    }
}