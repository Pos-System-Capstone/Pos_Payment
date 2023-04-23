namespace ResoPayment.Payload.Response;

public class GetPaymentTypeOfOrder
{
	public Guid OrderId { get; set; }
	public Guid PaymentProviderId { get; set; }
	public string PaymentProviderName { get; set; }
	public string PicUrl { get; set; }
}