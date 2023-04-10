namespace ResoPayment.Payload.Request
{
    public class PaymentInformationRequest
    {
	    public Guid OrderId { get; set; }
	    public string InvoiceId { get; set; }
        public string OrderType { get; set; }
        public double Amount { get; set; }
        public string OrderDescription { get; set; }
        public string Name { get; set; }
    }
}
