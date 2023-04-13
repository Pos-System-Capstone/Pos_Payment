using ResoPayment.Payload.Response;

namespace ResoPayment.PaymentStrategy;

public interface IPaymentStrategy
{
    CreatePaymentResponse ExecutePayment();
}