using ResoPayment.Payload.Request;

namespace ResoPayment.Service.Interfaces;

public interface IBrandService
{
	Task<bool> CreateBrandPaymentProviderMapping(CreateBrandPaymentProviderMappingRequest request);
}