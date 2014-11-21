using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Host
{
    [ServiceContract]
    class HelloService
    {
        [OperationContract]
        string HelloWorld(string name)
        {
            return string.Format("Hello {0}", name);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Type serviceType = typeof(HelloService);
            ServiceHost host = new ServiceHost(serviceType, new Uri[] { new Uri("http://localhost:8080") });
            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            behavior.HttpGetEnabled = true;
            host.Description.Behaviors.Add(behavior);

            host.AddServiceEndpoint(serviceType, new BasicHttpBinding(), "HelloService");
            host.Open();
            Console.WriteLine("Service is ready, press any key to terminate.");
            Console.ReadKey();
        }
    }
}
