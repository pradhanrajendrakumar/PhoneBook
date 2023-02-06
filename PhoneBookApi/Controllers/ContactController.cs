using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhoneBookApi.Models;
using PhoneBookApi.Services;
using PhoneBookApi.Services.RequestModels;
using PhoneBookApi.Services.ResponseModels;

namespace PhoneBookApi.Controllers
{
    [Route("api/v{version:apiVersion}/Contact")]
    [ApiVersion("1.0")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IContactService contactService,IMapper mapper, ILogger<ContactController> logger)
        {
            _contactService = contactService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result= await _contactService.GetContactListAsync();

            if (result.Success)
            {
                if (result != null)
                {
                    var contactList = _mapper.Map<List<ContactResponse>>(result.Data);
                    return Ok(contactList);
                }
                else
                { 
                    return NotFound(); 
                }
            }
            else
            {
                _logger.LogError(result.ErrorMessage);
                return BadRequest(result.ErrorMessage);
            }            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result= await _contactService.GetContactByIdAsync(id);

            if (result.Success)
            {
                if (result!=null)
                {
                    var contact= _mapper.Map<ContactResponse>(result.Data);
                    return Ok(contact);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateContact(ContactRequest contactRequest)
        {
            var contact = _mapper.Map<Contact>(contactRequest);
            var result= await _contactService.AddContactAsync(contact);
            if (result.Success)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, ContactResponse contactResponse)
        {
            if (id != contactResponse.Id.ToString())
            {
                return BadRequest("Id's are not matching.");
            }
            var existingContact = await _contactService.GetContactByIdAsync(id);
            if(existingContact==null)
            {
                return NotFound();
            }
            var contact = _mapper.Map<Contact>(contactResponse);
            var result = await _contactService.UpdateContact(contact);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existingContact = await _contactService.GetContactByIdAsync(id);
            if (existingContact.Data == null)
            {
                return NotFound();
            }
            var result= _contactService.Delete(id); 
            if (result.Success)
            {
                return Ok();
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }
    }
}
