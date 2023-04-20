using ResoPayment.Infrastructure.Models;

namespace ResoPayment.Payload.Response
{
    public class TransactionReportResponse
    {
        public int TotalTransaction   { get; set; }
        public double TotalAmount { get; set; }
        public List<String>? ListPaymentProvider  { get; set; }
        public List<double>? ListTotalAmountInPaymentProvider { get; set; }
        public List<int>? ListTotalTransactionInPaymentProvider { get; set; }

        public TransactionReportResponse(int totalTransaction, double totalAmount, List<string>? listPaymentProvider, List<double>? listTotalAmountInPaymentProvider, List<int>? listTotalTransactionInPaymentProvider)
        {
            TotalTransaction = totalTransaction;
            TotalAmount = totalAmount;
            ListPaymentProvider = listPaymentProvider;
            ListTotalAmountInPaymentProvider = listTotalAmountInPaymentProvider;
            ListTotalTransactionInPaymentProvider = listTotalTransactionInPaymentProvider;
        }
    }
}
