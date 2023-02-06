using PhoneBookApi.Models;

namespace PhoneBookApi.Repositories
{
    public interface IContactRepository
    {
        Task<List<Contact>> GetContactsAsync();
        Task<Contact> GetContactById(string contactId);
        Task InsertContactAsync(Contact contact);
        Task<bool> UpdateContactAsync(Contact contact);
        bool DeleteContact(string contactId);
    }
}