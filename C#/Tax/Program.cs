using System.IO;
using System.Linq;
using System;

namespace Tax
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            double salary = 1000D;
            using (var writer = new StreamWriter("Delta.CSV"))
            {
                writer.WriteLine("{0},{1},{2},{3}", "Bonus", "Original", "Best", "Delta");
                for (int bonus = 10000; bonus < 200000; bonus += 1000)
                {
                    double original = new Salary(salary).Net + new Bonus(bonus).Net;
                    double total = salary + bonus;
                    double delta = (total - 1000) / 1000;
                    var max = (from i in Enumerable.Range(0, 1000)
                               let s = 1000 + delta * i
                               let b = total - s
                               select new Salary(s).Net + new Bonus(b).Net).Max();

                    writer.WriteLine("{0},{1},{2},{3}", bonus, original, max, max - original);
                }
            }
        }

        private static void CalculateBestPoint()
        {
            double amount = 200000D;
            using (var writer = new StreamWriter("BestPoint.CSV"))
            {
                writer.WriteLine("{0},{1},{2}", "Salary", "Bonus", "Net");
                for (int i = 10; i < 2001; i++)
                {
                    double salary = i * 100D;
                    double bonus = amount - salary;

                    double net = new Salary(salary).Net + new Bonus(bonus).Net;

                    writer.WriteLine("{0},{1},{2}", salary, bonus, net);
                }
            }
        }
    }
}