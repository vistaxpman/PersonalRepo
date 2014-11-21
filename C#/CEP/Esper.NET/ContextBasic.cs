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
            var runtime = provider.EPRuntime;

            const string epl1 = "create context appleContext partition by Id from CepSamples.Apple";
            const string epl2 = "context appleContext select context.id, context.name, context.key1, avg(Price) from CepSamples.Apple";
            admin.CreateEPL(epl1);
            var statement = admin.CreateEPL(epl2);
            statement.Events += (sender, e) =>
                {
                    var evnt = e.NewEvents[0];
                    Console.WriteLine("context.name {0}, context.id {1}, context.key1 {2}, avg(Price) {3}",
                        evnt.Get("name"), evnt.Get("id"), evnt.Get("key1"), evnt.Get("avg(Price)"));
                };

            runtime.SendEvent(new Apple {Id = 1, Price = 5});

            runtime.SendEvent(new Apple { Id = 2, Price = 10 });

            runtime.SendEvent(new Apple { Id = 3, Price = 15 });

            runtime.SendEvent(new Apple { Id = 3, Price = 25 });
            
            Console.ReadLine();
        }
    }
}