using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FalseSharing
{
    internal class FalseSharing
    {
        private class FalseSharingData
        {
            public int field1;
            public int field2;
        }

        [StructLayout(LayoutKind.Explicit)]
        private class GoodData
        {
            [FieldOffset(0)]
            public int field1;

            [FieldOffset(64)]
            public int field2;
        }

        private static void Main(string[] args)
        {
            int iterations = 100000000;

            var badData = new FalseSharingData();
            var stopwatch = Stopwatch.StartNew();
            var t1 = Task.Factory.StartNew(() => AccessFalseSharingData(badData, 0, iterations));
            var t2 = Task.Factory.StartNew(() => AccessFalseSharingData(badData, 1, iterations));
            Task.WaitAll(t1, t2);
            Console.WriteLine("BadData  Access Time: {0} ms", stopwatch.ElapsedMilliseconds);

            var goodData = new GoodData();
            stopwatch.Restart();
            t1 = Task.Factory.StartNew(() => AccessGoodData(goodData, 0, iterations));
            t2 = Task.Factory.StartNew(() => AccessGoodData(goodData, 1, iterations));
            Task.WaitAll(t1, t2);
            Console.WriteLine("GoodData Access Time: {0} ms", stopwatch.ElapsedMilliseconds);

            Console.ReadLine();
        }

        private static void AccessGoodData(GoodData data, int field, int iterations)
        {
            for (int x = 0; x < iterations; x++)
            {
                if (field == 0)
                {
                    data.field1++;
                }
                else
                {
                    data.field2++;
                }
            }
        }

        private static void AccessFalseSharingData(FalseSharingData data, int field, int iterations)
        {
            for (int x = 0; x < iterations; x++)
            {
                if (field == 0)
                {
                    data.field1++;
                }
                else
                {
                    data.field2++;
                }
            }
        }
    }
}