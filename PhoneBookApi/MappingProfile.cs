using AutoMapper;
using PhoneBookApi.Models;
using PhoneBookApi.Services.RequestModels;
using PhoneBookApi.Services.ResponseModels;

namespace PhoneBookApi
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Contact, ContactResponse>().ReverseMap();
            CreateMap<ContactRequest, Contact>();
        }
    }
}
