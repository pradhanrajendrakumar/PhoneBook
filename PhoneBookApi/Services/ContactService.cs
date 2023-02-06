using PhoneBookApi.Models;
using PhoneBookApi.Repositories;

namespace PhoneBookApi.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public async Task<ServiceResponse<Contact>> AddContactAsync(Contact contact)
        {
            ServiceResponse<Contact> response = new ServiceResponse<Contact>();
            try
            {
                await _contactRepository.InsertContactAsync(contact);
                response.Success = true;
                response.Data= contact;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.Success = false;
            }

            return response;
        }

        public CommonServiceResponse Delete(string contactId)
        {
            CommonServiceResponse commonServiceResponse=new CommonServiceResponse();
            try
            {
                var result = _contactRepository.DeleteContact(contactId);
                commonServiceResponse.Success = true;

            }
            catch (Exception ex)
            {
                commonServiceResponse.ErrorMessage = ex.Message;
                commonServiceResponse.Success = false;
            }

            return commonServiceResponse;
        }

        public async Task<ServiceResponse<Contact>> GetContactByIdAsync(string contactId)
        {
            ServiceResponse<Contact> serviceResponse= new ServiceResponse<Contact>();

            try
            {
                var result = await _contactRepository.GetContactById(contactId);
                serviceResponse.Success = true;
                if (result!=null)
                {
                    serviceResponse.Data= result;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.ErrorMessage = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<IEnumerable<Contact>>> GetContactListAsync()
        {
            ServiceResponse<IEnumerable<Contact>> serviceResponse= new ServiceResponse<IEnumerable<Contact>>();
            try
            {
                var result= await _contactRepository.GetContactsAsync();
                serviceResponse.Success = true;
                if (result!=null && result.Any())
                {
                    serviceResponse.Data= result;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.ErrorMessage= ex.Message;
                serviceResponse.Success =false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Contact>> UpdateContact(Contact contact)
        {
            ServiceResponse<Contact> serviceResponse = new ServiceResponse<Contact>();
            try
            {
                var result = await _contactRepository.UpdateContactAsync(contact);
                if (result)
                {
                    serviceResponse.Data = contact;
                    serviceResponse.Success = true;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.HasError= true;
                serviceResponse.ErrorMessage = ex.Message;
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }
    }
}
