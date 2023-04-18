using ResoPayment.Enums;

namespace ResoPayment.Payload.Response
{
    public class CreatePaymentResponse
    {
        public string? Message { get; set; }
        public string Url { get; set; }
        public CreatePaymentReturnType DisplayType { get; set; }

    }
}
