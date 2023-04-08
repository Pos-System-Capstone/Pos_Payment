using ResoPayment.Payload.Request;
using ResoPayment.Payload.Response;

namespace ResoPayment.Service.Interfaces
{
    public interface IVnPayServices
    {


        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);


        PaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}
