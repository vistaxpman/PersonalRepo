using System;
using System.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var candicates = Enumerable.Range(2, 99).ToList();

            Enumerable.Range(2, 9).ToList().ForEach(factor =>
                candicates.RemoveAll(x =>
                    (x > factor) && (x % factor == 0))
            );
            candicates.ForEach(x =>
                Console.WriteLine(x));
        }
    }
}