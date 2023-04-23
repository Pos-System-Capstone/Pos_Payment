using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ResoPayment.Constants;
using ResoPayment.Payload.Request;
using ResoPayment.Payload.Response;
using ResoPayment.Service.Implements;
using ResoPayment.Service.Interfaces;
using System.Net;

namespace ResoPayment.Controllers
{
    [ApiController]
    public class PaymentsController : BaseController<PaymentsController>
    {
        private readonly IVnPayServices _vnPayService;
        private readonly ITransactionService _transactionService;
        private readonly IZaloPayServices _zaloPayServices;
        private readonly IPaymentProviderService _paymentProviderService;
        public PaymentsController(ILogger<PaymentsController> logger, IVnPayServices vnPayService, ITransactionService transactionService, IZaloPayServices zaloPayServices,IPaymentProviderService paymentProviderService) : base(logger)
        {
            _vnPayService = vnPayService;
            _transactionService = transactionService;
            _zaloPayServices = zaloPayServices;
            _paymentProviderService = paymentProviderService;
        }

        [Authorize]
        [HttpPost(ApiEndPointConstant.Payment.PaymentEndpoint)]
        [ProducesResponseType(typeof(CreatePaymentResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePaymentUrl(CreatePaymentRequest createPaymentRequest)
        { 
	        var url = await _transactionService.CreatePayment(createPaymentRequest);
            return Ok(url);
        }

        [HttpGet(ApiEndPointConstant.Payment.VnPayEndpoint)]
        public async Task<IActionResult> PaymentCallBack(string? vnp_Amount, string? vnp_BankCode,
            string? vnp_BankTranNo, string? vnp_CardType, string? vnp_OrderInfo, string? vnp_PayDate,
            string? vnp_ResponseCode, string? vnp_TmnCode, string? vnp_TransactionNo, string? vnp_TxnRef,
            string? vnp_SecureHashType, string? vnp_SecureHash)
        {
			bool isSuccessful = await _transactionService.ExecuteVnPayCalBack(vnp_Amount, vnp_BankCode, vnp_BankTranNo,
				vnp_CardType, vnp_OrderInfo, vnp_PayDate, vnp_ResponseCode, vnp_TmnCode, vnp_TransactionNo, vnp_TxnRef,
				vnp_SecureHashType, vnp_SecureHash);
			if (isSuccessful)
			{
				return RedirectPermanent("https://firebasestorage.googleapis.com/v0/b/pos-system-47f93.appspot.com/o/files%2Fpayment-done.png?alt=media&token=284c1b35-e4f2-417e-90e4-a339c4cd7a4e");
			}
			else
			{
				return RedirectPermanent("https://firebasestorage.googleapis.com/v0/b/pos-system-47f93.appspot.com/o/files%2Fpayment-fail.png?alt=media&token=2b7e58ee-c18f-4ec3-9363-ad1ec83ffc6c");
			}
        }

        [HttpGet(ApiEndPointConstant.Payment.ZaloPayEndpoint)]
        public async Task<IActionResult> ZaloPayPaymentCallBack(double? amount, double? discountamount, string? appid, string? checksum, string? apptransid, int? status)
        {
            var isSuccessful = await _transactionService.ExecuteZaloPayCallBack(amount, discountamount, appid, checksum, apptransid,
	            status);

            if (isSuccessful)
            {
                return RedirectPermanent("https://firebasestorage.googleapis.com/v0/b/pos-system-47f93.appspot.com/o/files%2Fpayment-done.png?alt=media&token=284c1b35-e4f2-417e-90e4-a339c4cd7a4e");
            }
            else
            {
                return RedirectPermanent("https://firebasestorage.googleapis.com/v0/b/pos-system-47f93.appspot.com/o/files%2Fpayment-fail.png?alt=media&token=2b7e58ee-c18f-4ec3-9363-ad1ec83ffc6c");
            }
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.Payment.PaymentProviderEndpoint)]
        [ProducesResponseType(typeof(PaymentProviderResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListPaymentProviderInBrand()
        {
            var res = await _paymentProviderService.GetAllPaymentTypesByBrandId();
            return Ok(res);
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.Payment.CheckTransactionStatus)]
        public async Task<IActionResult> CheckTransactionStatus([FromQuery] string orderId)
        {
	        var result = await _transactionService.CheckTransactionStatus(orderId);
            return Ok(result);
        }

		[Authorize]
		[HttpGet(ApiEndPointConstant.Payment.PaymentEndpoint)]
        public async Task<IActionResult> GetPaymentTypeOfOrder([FromQuery] Guid orderId)
        {
	        var response = await _transactionService.GetPaymentTypeOfOrder(orderId);
	        return Ok(response);
        }
    }
}
