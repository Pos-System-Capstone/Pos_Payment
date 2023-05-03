using ResoPayment.Payload.Request;

namespace ResoPayment.Payload.Response;

public class GetBrandPaymentProviderMappingResponse
{
	public Guid BrandId { get; set; }
	public string BrandName { get; set; }
	public string BrandPhoneNumber { get; set; }
	public VietQRConfigRequest? VietQrConfigRequest { get; set; }
	public ZaloPayConfigRequest? ZaloPayConfigRequest { get; set; }
	public VnPayConfigRequest? VnPayConfigRequest { get; set; }
}