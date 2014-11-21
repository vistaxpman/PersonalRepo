using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            HelloServiceClient client = new HelloServiceClient();
            Console.WriteLine(client.HelloWorld("WCF"));
        }
    }
}