using System;
using System.Net;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

// Sample for Reactive Framework/Rx.
namespace Rx
{
    internal class Rx
    {
        public static event EventHandler<EventArgs> GenericEvent;

        private static void Main(string[] args)
        {
            //Sample1();
            //Console.WriteLine("The content length of http://Wintellect.com/ is {0}.", Sample2().Result.ContentLength);
            Sample3().Wait();
            Sample4();
        }

        private static void Sample1()
        {
            IObservable<int> source;

            //source= Observable.Empty<int>();
            //source = Observable.Throw<int>(new Exception("Oops"));
            //source = Observable.Return(42);
            //source = Observable.Range(5, 3);
            //source = Observable.Generate(0, i => i < 5, i => i + 1, i => i * i);
            source = Observable.Generate(0, i => i < 5, i => i + 1, i => i * i, i => TimeSpan.FromSeconds(i));

            using (source.Subscribe(
                x => Console.WriteLine("OnNext: {0}", x),
                ex => Console.WriteLine("OnError: {0}", ex.Message),
                () => Console.WriteLine("OnCompleted")))
            {
                Console.WriteLine("Press ENTER to unsubscribe...");
                Console.ReadLine();
            }
        }

        private static async Task<WebResponse> Sample2()
        {
            Uri uri;
            Uri.TryCreate("http://Wintellect.com/", UriKind.Absolute, out uri);
            var webRequest = WebRequest.Create(uri);
            var getResponse = Observable.FromAsync(webRequest.GetResponseAsync);

            return await getResponse.FirstAsync();
        }

        private static async Task Sample3()
        {
            var o1 = Observable.Generate(0, i => i < 9, i => ++i, i => Thread.CurrentThread.ManagedThreadId, _ => TimeSpan.FromSeconds(1));
            var o2 = Observable.Generate(0, i => i < 9, i => ++i, i => Thread.CurrentThread.ManagedThreadId);

            var result = o1.Zip(o2,
                (first, second) => string.Format("o1 Running in {0}.\no2 Running in {1}\n.", first, second));

            await result.ForEachAsync(r => Console.WriteLine(r));
        }

        private static void Sample4()
        {
            var lbl = new Label();
            var frm = new Form { Controls = { lbl } };
            var postions = from e in Observable.FromEventPattern<MouseEventArgs>(frm, "MouseMove")
                           select e.EventArgs.Location;

            using (postions.Subscribe(p => { lbl.Text = p.ToString(); }))
            {
                Application.Run(frm);
            }
        }
    }
}