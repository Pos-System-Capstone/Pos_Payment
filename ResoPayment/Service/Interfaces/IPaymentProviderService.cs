using ResoPayment.Payload.Response;

namespace ResoPayment.Service.Interfaces
{
    public interface IPaymentProviderService
    {
        Task<IEnumerable<PaymentProviderResponse>> GetAllPaymentTypesByBrandId();
    }
}
