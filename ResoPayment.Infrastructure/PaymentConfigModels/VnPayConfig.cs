namespace ResoPayment.Infrastructure.PaymentConfigModels;

public class VnPayConfig
{
	public string BaseUrl { get; set; }
	public string Version { get; set; }
	public string Command { get; set; }
	public string TmnCode { get; set; }	
	public string CurrCode { get; set; }
	public string SecureHash { get; set; }
}