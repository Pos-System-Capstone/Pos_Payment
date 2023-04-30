using ResoPayment.Payload.Request;
using ResoPayment.Payload.Response;
using ResoPayment.RedisModels;

namespace ResoPayment.Service.Interfaces;

public interface ITransactionService
{
	Task<bool> ExecuteVnPayCalBack(string? vnp_Amount, string? vnp_BankCode,
		string? vnp_BankTranNo, string? vnp_CardType, string? vnp_OrderInfo, string? vnp_PayDate,
		string? vnp_ResponseCode, string? vnp_TmnCode, string? vnp_TransactionNo, string? vnp_TxnRef,
		string? vnp_SecureHashType, string? vnp_SecureHash);

	Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest createPaymentRequest);

    Task<bool> ExecuteZaloPayCallBack(double? amount, double? discountamount, string? appid, string? checksum,
	    string? apptransid, int? status);

    Task<OrderData> CheckTransactionStatus(string orderid);
    Task<TransactionReportResponse> GetTransactionReportAsync();

	Task<GetPaymentTypeOfOrder> GetPaymentTypeOfOrder(Guid orderId);
}