using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AmericanPresidentialElection
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var states = new int[] { 1, 2, 3, 4 };
            JudgeEquality(states);
            Debug.Assert(JudgeEquality(states) == JudgeEqualityRecursively(states));
        }

        private static bool JudgeEqualityRecursively(int[] states)
        {
            int total = states.Sum();

            if (total % 2 == 1)
            {
                return false;
            }

            return JudgeEqualityRecursively(states, total / 2, 0);
        }

        private static bool JudgeEqualityRecursively(IEnumerable<int> states, int total, int sum)
        {
            if (!states.Any())
            {
                return false;
            }

            if (sum == total)
            {
                return true;
            }

            foreach (var state in states)
            {
                if (JudgeEqualityRecursively(states.Except(new[] { state }), total, sum + state))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool JudgeEquality(int[] states)
        {
            int total = states.Sum();
            if (total % 2 == 1)
                return false;
            int target = total >> 1;
            for (ulong i = 1; i < Math.Pow(2, states.Count()) - 1; ++i)
            {
                int sum = 0;
                ulong j = i;
                for (int k = 0; k < states.Count(); ++k)
                {
                    if (j % 2 == 1)
                    {
                        sum += states[k];
                    }
                    if (sum == target)
                    {
                        return true;
                    }
                    j = j >> 1;
                }
            }

            return false;
        }
    }
}