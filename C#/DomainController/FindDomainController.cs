using System;
using System.DirectoryServices.ActiveDirectory;

namespace FindDomainController
{
    internal class FindDomainController
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("<-- PRCM LOCAL DOMAIN CONTROLLER -->");
                var domainContext = new DirectoryContext(DirectoryContextType.Domain);
                var domainController = DomainController.FindOne(domainContext);
                Console.WriteLine("Local Domain in the PRCM domain: {0}", domainController);
                Console.WriteLine("Site Name: {0}", domainController.SiteName);
                Console.WriteLine("IP Address: {0}\n", domainController.IPAddress);
            }
            catch (ActiveDirectoryObjectNotFoundException e)
            {
                // DomainController not found
                Console.WriteLine(e.Message);
            }
        }
    }
}