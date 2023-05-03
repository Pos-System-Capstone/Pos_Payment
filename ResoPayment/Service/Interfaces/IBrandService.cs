using ResoPayment.Payload.Request;
using ResoPayment.Payload.Response;

namespace ResoPayment.Service.Interfaces;

public interface IBrandService
{
	Task<bool> CreateBrandPaymentProviderMapping(CreateBrandPaymentProviderMappingRequest request);

	Task<GetBrandPaymentProviderMappingResponse> GetBrandPaymentProviderMapping(Guid brandId);
}