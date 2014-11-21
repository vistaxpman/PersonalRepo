using System;

namespace RethrowExTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Whoops! Re-throw from procedural code loses the line number of the exception");
            try
            {
                try
                {
                    int zero = 0;
                    int i = 1 / zero;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Inner catch:");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Outer catch:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            Console.WriteLine();
            Console.WriteLine("However, re-throw from exception-throwing method retains the line number, as expected");
            try
            {
                try
                {
                    MethodThatThrowsException();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Inner catch:");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Outer catch:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            Console.WriteLine();
            Console.WriteLine("New throw from procedural code loses the line number, as expected");
            try
            {
                try
                {
                    int zero = 0;
                    int i = 1 / zero;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Inner catch:");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Outer catch:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            Console.WriteLine();
            Console.WriteLine("New throw from method loses the line number, as expected");
            try
            {
                try
                {
                    MethodThatThrowsException();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Inner catch:");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Outer catch:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static void MethodThatThrowsException()
        {
            int zero = 0;
            int i = 1 / zero;
        }
    }
} 