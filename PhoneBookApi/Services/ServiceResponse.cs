namespace PhoneBookApi.Services
{
    public class ServiceResponse<T>:CommonServiceResponse
    {
        public ServiceResponse() { }

        public T? Data { get; set; }
    }
}