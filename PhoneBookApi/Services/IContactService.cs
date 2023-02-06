using PhoneBookApi.Models;

namespace PhoneBookApi.Services
{
    public interface IContactService
    {
        Task<ServiceResponse<IEnumerable<Contact>>> GetContactListAsync();
        CommonServiceResponse Delete(string contactId);
        Task<ServiceResponse<Contact>> GetContactByIdAsync(string contactId);
        Task<ServiceResponse<Contact>> UpdateContact(Contact contact);
        Task<ServiceResponse<Contact>> AddContactAsync(Contact contact);
    }
}
