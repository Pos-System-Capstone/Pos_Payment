namespace ResoPayment.Payload.Request;

public class CreateBrandPaymentProviderMappingRequest
{
	public Guid BrandId { get; set; }
	public string BrandName { get; set; }
	public string BrandPhoneNumber { get; set; }
	public List<CreateStoreRequest> CreateStoreRequests { get; set; }
	public VietQRConfigRequest? VietQrConfigRequest { get; set; }
	public ZaloPayConfigRequest? ZaloPayConfigRequest { get; set; }
	public VnPayConfigRequest? VnPayConfigRequest { get; set; }


}

public class CreateStoreRequest
{
	public Guid StoreId { get; set; }
	public string StoreName { get; set; }
	public string? StoreAddress { get; set; }
	public string? StorePhoneNumber { get; set; }
	public string? StoreEmail { get; set; }
}

public class VietQRConfigRequest
{
	public string BankCode { get; set; }
	public string AccountNumber { get; set; }
	public string AccountName { get; set; }
}

public class ZaloPayConfigRequest
{
	public string AppId { get; set; }
	public string Key1 { get; set; }
	public string Key2 { get; set; }
}

public class VnPayConfigRequest
{
	public string TmnCode { get; set; }
	public string SecureHash { get; set; }
}