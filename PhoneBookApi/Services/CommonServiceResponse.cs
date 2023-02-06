namespace PhoneBookApi.Services
{
    public class CommonServiceResponse
    {
        public CommonServiceResponse() { }

        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }

        public bool HasError { get; set; }
    }
}