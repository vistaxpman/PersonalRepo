using System;
using System.Collections.Generic;

namespace NumberBlackHole
{
    public class Entry
    {
        public Entry(int x, int y)
        {
            X = Math.Max(x, y);
            Y = Math.Min(x, y);
        }

        public int X { get; set; }

        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            var right = (Entry)obj;
            return GetHashCode() == right.GetHashCode();
        }

        public override int GetHashCode()
        {
            return X * 10 + Y;
        }

        public override string ToString()
        {
            return GetHashCode().ToString();
        }
    }

    public class Program
    {
        private static void Main(string[] args)
        {
            var dictionary = new Dictionary<Entry, Entry>();

            for (var i = 0; i < 10; i++)
                for (var j = 0; j < i; j++)
                {
                    var key = new Entry(i, j);
                    var value = (i * 10 + j) - (j * 10 + i);
                    dictionary[key] = new Entry(value / 10, value % 10);
                }

            for (var i = 0; i < 10; i++)
                for (var j = 0; j < i; j++)
                {
                    var key = new Entry(i, j);
                    Find(key, dictionary);

                    Console.WriteLine();
                }
        }

        private static void Find(Entry key, Dictionary<Entry, Entry> dictionary)
        {
            Console.Write(string.Format("{0} -> ", key));
            Entry value;
            if (!dictionary.TryGetValue(key, out value) || value.Y == 0)
            {
                if (value.Y == 0)
                    Console.Write(value.X);
                else
                    Console.Write("NULL");
                return;
            }

            Find(value, dictionary);
        }
    }
}