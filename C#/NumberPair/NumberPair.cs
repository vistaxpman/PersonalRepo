using System;
using System.Diagnostics;
using System.Linq;

namespace NumberPair
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Debug.Assert(BuildPair(12) == true);
            Debug.Assert(BuildPair(11) == true);
            Debug.Assert(BuildPair(10) == false);
            Debug.Assert(BuildPair(9) == false);
            Debug.Assert(BuildPair(8) == true);
            Debug.Assert(BuildPair(7) == true);
            Debug.Assert(BuildPair(6) == false);
            Debug.Assert(BuildPair(5) == false);
            Debug.Assert(BuildPair(4) == true);
            Debug.Assert(BuildPair(3) == true);
            Debug.Assert(BuildPair(2) == false);
            Debug.Assert(BuildPair(1) == false);
        }

        private static bool BuildPair(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentException("n must be greater than zero.");
            }

            if (n == 1)
            {
                return false;
            }

            // Initialize buffer
            int[] buffer = new int[2 * n];
            for (int i = 0; i < buffer.Length; ++i)
            {
                buffer[i] = 0;
            }

            // The first number must be n.
            buffer[0] = buffer[n + 1] = n;

            int[] availableNumbers = Enumerable.Range(1, n - 1).ToArray();
            bool result = Calculate(buffer, 1, availableNumbers);
            if (result)
            {
                Debug.WriteLine(buffer.Aggregate("", (sentence, next) => string.Format("{0}.{1}", sentence, next)));
            }
            return result;
        }

        private static bool Calculate(int[] buffer, int position, int[] availableNumbers)
        {
            // All numbers are insert. SUCESSFUL!
            if (availableNumbers.Length == 0)
            {
                return true;
            }

            foreach (var value in availableNumbers)
            {
                // The position already has value. Find next empty position.
                while (buffer[position] != 0)
                {
                    ++position;
                }

                if (position >= buffer.Length)
                {
                    return false;
                }

                int pairIndex = position + value + 1;
                if (pairIndex >= buffer.Length)
                {
                    return false;
                }

                // The pair position are occupied. try next value.
                if (buffer[pairIndex] != 0)
                {
                    continue;
                }

                buffer[position] = buffer[pairIndex] = value;
                bool result = Calculate(buffer, position + 1, availableNumbers.Where(v => v != value).ToArray());
                if (result)
                {
                    return true;
                }
                else
                {
                    // Failed. Recover the buffer.
                    buffer[position] = buffer[pairIndex] = 0;
                }
            }

            return false;
        }
    }
}