using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace TPLDataflow
{
    internal class TPLDataflow
    {
        [STAThread]
        private static void Main(string[] args)
        {
            //Sample1();
            Sample2();
        }

        private static void Sample1()
        {
            var blockOptions = new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = 4, };

            var displayBlock = new ActionBlock<int>(i =>
                {
                    Thread.Sleep(2000);
                    Console.WriteLine(i);
                }, blockOptions);

            var transformBlock = new TransformBlock<int, int>(i => i * 3);

            transformBlock.LinkTo(displayBlock);

            for (int i = 0; i < 20; i++)
            {
                transformBlock.Post(i);
            }

            Console.ReadLine();
        }

        private static void Sample2()
        {
            var application = new Application();
            var window = new Window() { Height = 600, Width = 800, };
            var stackPanel = new System.Windows.Controls.StackPanel();
            window.Content = stackPanel;
            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(application.Dispatcher));

            var urls = new[]
                {
                    "http://weblogs.asp.net/blogs/nmarun/image_thumb_283DC50D.png",
                    "http://weblogs.asp.net/blogs/nmarun/image_thumb_09FFF1D5.png",
                    "http://weblogs.asp.net/blogs/nmarun/image_thumb_420B7DCA.png",
                    "http://weblogs.asp.net/blogs/nmarun/image_thumb_5615AE0E.png",
                };

            var imageDownloader = new TransformBlock<string, System.Drawing.Image>(url => DownloadImage(url));
            var bitmapSourceCreater = new TransformBlock<System.Drawing.Image, BitmapSource>(image =>
                {
                    var bitmap = new System.Drawing.Bitmap(image);
                    var bmpPtr = bitmap.GetHbitmap();
                    var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bmpPtr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                    //freeze bitmapSource and clear memory to avoid memory leaks
                    bitmapSource.Freeze();
                    DeleteObject(bmpPtr);

                    return bitmapSource;
                });
            var blockOptions = new ExecutionDataflowBlockOptions { TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext(), };
            var uiOperator = new ActionBlock<BitmapSource>(bitmapSource =>
                {
                    var image = new System.Windows.Controls.Image() { Source = bitmapSource, Height = 100, };
                    stackPanel.Children.Add(image);
                }, blockOptions);

            imageDownloader.LinkTo(bitmapSourceCreater);
            bitmapSourceCreater.LinkTo(uiOperator);

            foreach (string url in urls)
            {
                imageDownloader.Post(url);
            }

            application.Run(window);
        }

        private static System.Drawing.Image DownloadImage(string imageUrl)
        {
            var request = WebRequest.Create(imageUrl);
            var response = request.GetResponse();

            return System.Drawing.Image.FromStream(response.GetResponseStream());
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject(IntPtr value);
    }
}