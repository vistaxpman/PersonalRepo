using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComplexTask
{
    internal class ComplexTask
    {
        private static void Main(string[] args)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            var task = Task<int>.Factory.StartNew(obj =>
                {
                    int n = (int)obj;
                    int sum = 0;

                    for (; n > 0; n--)
                    {
                        token.ThrowIfCancellationRequested();
                        checked
                        {
                            sum += n;
                        }
                        Console.WriteLine(n);
                    }
                    return sum;
                }, 100000, token);

            task.ContinueWith(t => Console.WriteLine("The sum is: {0}.", t.Result),
                TaskContinuationOptions.OnlyOnRanToCompletion);

            task.ContinueWith(t =>
                Console.WriteLine("The action is failed with {0}.", t.Exception.InnerException.GetType()),
                TaskContinuationOptions.OnlyOnFaulted);

            task.ContinueWith(t => Console.WriteLine("The action is canceled."),
                TaskContinuationOptions.OnlyOnCanceled);

            Console.ReadLine();
            tokenSource.Cancel();
            Console.ReadLine();
        }
    }
}