using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace APM
{
    internal class APM
    {
        // Asynchronous Programming Model - APM
        private static void Main(string[] args)
        {
            Uri uri;
            Uri.TryCreate("http://Wintellect.com/", UriKind.Absolute, out uri);

            MeasurePerformance(SyncRequestWebUri, uri);
            MeasurePerformance(AsyncRequestWebUriByAPM, uri);
            MeasurePerformance(AsyncRequestWebByTask, uri);
            MeasurePerformance(AsyncRequestWebByAsync, uri);
            Console.ReadLine();
        }

        private static void MeasurePerformance(Action<Uri> requestWebAction, Uri uri)
        {
            var stopwatch = Stopwatch.StartNew();
            requestWebAction(uri);
            Console.WriteLine("{0}: {1}ms.\n", requestWebAction.Method.Name, stopwatch.ElapsedMilliseconds);
        }

        #region SyncRequestWebUri

        private static void SyncRequestWebUri(Uri requestUri)
        {
            var webRequest = WebRequest.Create(requestUri);
            try
            {
                using (var webResponse = webRequest.GetResponse())
                {
                    Console.WriteLine("Content length: {0}", webResponse.ContentLength);
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine("The action is failed: {0}.", ex.Message);
            }
        }

        #endregion SyncRequestWebUri

        #region AsyncRequestWebUriByAPM

        private static void AsyncRequestWebUriByAPM(Uri requestUri)
        {
            var webRequest = WebRequest.Create(requestUri);
            var result = CreateAsyncAPMRequest(webRequest);
            result.AsyncWaitHandle.WaitOne();
        }

        private static IAsyncResult CreateAsyncAPMRequest(WebRequest webRequest)
        {
            return webRequest.BeginGetResponse(result =>
                {
                    WebResponse webResponse = null;
                    try
                    {
                        webResponse = webRequest.EndGetResponse(result);
                        Console.WriteLine("Content length: {0}", webResponse.ContentLength);
                    }
                    catch (WebException ex)
                    {
                        Console.WriteLine("The action is failed: {0}.", ex.Message);
                    }
                    finally
                    {
                        if (webResponse != null)
                        {
                            webResponse.Close();
                        }
                    }
                }, null);
        }

        #endregion AsyncRequestWebUriByAPM

        #region AsyncRequestWebByTask

        private static void AsyncRequestWebByTask(Uri requestUri)
        {
            var webRequest = WebRequest.Create(requestUri);
            var task = CreateAsyncRequestWebTask(webRequest);
            task.Wait();
        }

        private static Task CreateAsyncRequestWebTask(WebRequest webRequest)
        {
            return Task.Factory.FromAsync<WebResponse>(webRequest.BeginGetResponse, webRequest.EndGetResponse, null, TaskCreationOptions.None)
                .ContinueWith(task =>
                    {
                        WebResponse webResponse = null;
                        try
                        {
                            webResponse = task.Result;
                            Console.WriteLine("Content length: " + webResponse.ContentLength);
                        }
                        catch (AggregateException ex)
                        {
                            if (ex.GetBaseException() is WebException)
                                Console.WriteLine("The action is failed: {0}.", ex.GetBaseException().Message);
                            else throw;
                        }
                        finally
                        {
                            if (webResponse != null)
                            {
                                webResponse.Close();
                            }
                        }
                    });
        }

        #endregion AsyncRequestWebByTask

        #region AsyncRequestWebByAsync

        private static void AsyncRequestWebByAsync(Uri requestUri)
        {
            try
            {
                var task = AsyncRequestWeb(requestUri);
                Console.WriteLine("Content length: {0}", task.Result.Length);
            }
            catch (WebException ex)
            {
                Console.WriteLine("The action is failed: {0}.", ex.Message);
            }
        }

        private static async Task<byte[]> AsyncRequestWeb(Uri requestUri)
        {
            var webClient = new WebClient();
            return await webClient.DownloadDataTaskAsync(requestUri);
        }

        #endregion AsyncRequestWebByAsync
    }
}