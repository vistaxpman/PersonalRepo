using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Async
{
    internal class Async
    {
        private static void Main(string[] args)
        {
            AsyncWhenAll().Wait();
            Console.ReadLine();
        }

        private static async Task<byte[]> AsyncSample()
        {
            Uri uri;
            Uri.TryCreate("http://www.google.com", UriKind.Absolute, out uri);
            var webClient = new WebClient();
            return await webClient.DownloadDataTaskAsync(uri);
        }

        private static async Task AsyncWhenAll()
        {
            Uri[] uris = { new Uri("http://www.google.com"), new Uri("http://www.bing.com/"), new Uri("http://www.baidu.com") };

            var webClient = new WebClient();
            var stopwatch = Stopwatch.StartNew();
            foreach (var uri in uris)
            {
                var innerStopwatch = Stopwatch.StartNew();
                var d = await webClient.DownloadDataTaskAsync(uri);
                Console.WriteLine("The content lenght is {0}", d.Length);
                Console.WriteLine("{0} {1} ms", uri, innerStopwatch.ElapsedMilliseconds);
            }
            Console.WriteLine("Sync: {0} ms", stopwatch.ElapsedMilliseconds);

            stopwatch.Restart();
            var asyncData = await Task.WhenAll(from uri in uris select new WebClient().DownloadDataTaskAsync(uri));
            foreach (var d in asyncData)
            {
                Console.WriteLine("The content lenght is {0}", d.Length);
            }
            Console.WriteLine("Async: {0} ms", stopwatch.ElapsedMilliseconds);
        }
    }
}