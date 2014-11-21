
namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            EmailValidatorClient svc = new EmailValidatorClient();
            bool result = svc.ValidateAddress("dennis@bloggingabout.net");
        }
    }
}
