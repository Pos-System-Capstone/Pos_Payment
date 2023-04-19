using ResoPayment.Enums;

namespace ResoPayment.RedisModels;

public class OrderData
{
	public Guid Id { get; set; }
	public TransactionStatus TransactionStatus { get; set; }
}