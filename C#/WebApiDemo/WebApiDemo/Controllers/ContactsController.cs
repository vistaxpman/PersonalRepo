using System.Linq;
using System.Net;
using System.Web.Http;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    public class ContactsController : ApiController
    {
        private static readonly IContactRepository _contacts = new ContactRepository(string.Empty);

        public IQueryable<Contact> Get()
        {
            return _contacts.GetAllContacts().AsQueryable();
        }

        public Contact Get(string id)
        {
            Contact contact = _contacts.GetContact(id);
            if (contact == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return contact;
        }

        public Contact Post(Contact value)
        {
            Contact contact = _contacts.AddContact(value);
            return contact;
        }

        public void Put(string id, Contact value)
        {
            if (!_contacts.UpdateContact(id, value))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        public void Delete(string id)
        {
            if (!_contacts.RemoveContact(id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}