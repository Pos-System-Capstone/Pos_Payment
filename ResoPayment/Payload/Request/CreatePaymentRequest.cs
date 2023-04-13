namespace ResoPayment.Payload.Request
{
    public class CreatePaymentRequest
    {
	    public Guid OrderId { get; set; }
	    public string InvoiceId { get; set; }
	    public Guid StoreId { get; set; }
	    public Guid AccountId { get; set; }
        public string PaymentType { get; set; }
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
    }
}
