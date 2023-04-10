using System.ComponentModel;

namespace ResoPayment.Enums;

public enum TransactionStatus
{
	[Description("Pending")]
	Pending,
	[Description("Paid")]
	Paid,
	[Description("Fail")]
	Fail
}