
namespace RestServiceSample
{
    public class RestService : IRestService
    {
        public string XmlData(string id)
        {
            return "You request the product: " + id;
        }

        public string JasonData(string id)
        {
            return "You request the product: " + id;
        }
    }
}
