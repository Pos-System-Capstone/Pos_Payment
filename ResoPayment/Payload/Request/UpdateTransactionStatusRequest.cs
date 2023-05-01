using ResoPayment.Enums;

namespace ResoPayment.Payload.Request;

public class UpdateTransactionStatusRequest
{
	public Guid OrderId { get; set; }
	public TransactionStatus TransactionStatus { get; set; }
}