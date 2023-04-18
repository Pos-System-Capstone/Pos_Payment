using ResoPayment.Payload.Request;

namespace ResoPayment.Service.Interfaces
{
    public interface IZaloPayServices
    {
        public Task<Dictionary<string, object>> CreatePayment(HttpContext context);

        public Task<int> ExecutedCallback();

    }
}
