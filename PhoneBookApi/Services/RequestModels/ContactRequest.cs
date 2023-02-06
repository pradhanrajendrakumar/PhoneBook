﻿namespace PhoneBookApi.Services.RequestModels
{
    public class ContactRequest
    {
       
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Image { get; set; }
    }
}
