using System.ServiceModel;

namespace EmailService
{
    [ServiceContract] 
    public interface IEmailValidator
    {
        [OperationContract]
        bool ValidateAddress(string emailAddress);  
    }
}
