using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace RestServiceSample
{
    // Start the service and browse to http://<machine_name>:<port>/TimeService/help to view the service's generated help page
    // NOTE: By default, a new instance of the service is created for each call; change the InstanceContextMode to Single if you want
    // a single instance of the service to process all calls.	
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class TimeService
    {
        [WebGet(UriTemplate = "CurrentTime")]
        public string CurrentTime()
        {
            return DateTime.Now.ToString();
        }
    }
}
