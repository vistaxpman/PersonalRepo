using System;
using System.Runtime.Remoting.Messaging;

namespace Test
{
    class Program
    {
        static event EventHandler TestEvent;

        static void Main(string[] args)
        {
            TestEvent +=
                (object sender, EventArgs e) =>
                {
                    Console.WriteLine("D1");
                    throw new Exception();
                };
            TestEvent.BeginInvoke(null, EventArgs.Empty,
                (result) =>
                {
                    var handler = (EventHandler)((AsyncResult)result).AsyncDelegate;
                    handler.EndInvoke(result);
                }, null
            );
        }
    }
}
