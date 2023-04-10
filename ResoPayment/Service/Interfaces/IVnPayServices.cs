using ResoPayment.Payload.Request;
using ResoPayment.Payload.Response;

namespace ResoPayment.Service.Interfaces
{
    public interface IVnPayServices
    {


	    Task<string> CreatePaymentUrl(PaymentInformationRequest model, HttpContext context);


        PaymentResponseModel PaymentExecute(IQueryCollection collections);

    }
}
