using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResoPayment.Constants;
using ResoPayment.Payload.Request;
using ResoPayment.Service.Interfaces;

namespace ResoPayment.Controllers
{
    [ApiController]
    public class PaymentsController : BaseController<PaymentsController>
    {
        private readonly IVnPayServices _vnPayService;
        private readonly ITransactionService _transactionService;
        private readonly IZaloPayServices _zaloPayServices;
        public PaymentsController(ILogger<PaymentsController> logger, IVnPayServices vnPayService, ITransactionService transactionService, IZaloPayServices zaloPayServices) : base(logger)
        {
            _vnPayService = vnPayService;
            _transactionService = transactionService;
            _zaloPayServices = zaloPayServices;
        }

        [Authorize]
        [HttpPost(ApiEndPointConstant.Payment.ZaloPayEndpoint)]
        //Hard-code for VN-PAY payment. Need to be refactored after
        public async Task<IActionResult> CreatePayment()
        {
            var url = await _zaloPayServices.CreatePayment(HttpContext);
            return Ok(url);
        }
    
		[Authorize]
		[HttpPost(ApiEndPointConstant.Payment.PaymentEndpoint)]
		//Hard-code for VN-PAY payment. Need to be refactored after
		public async Task<IActionResult> CreatePaymentUrl(CreatePaymentRequest createPaymentRequest)
		{
			var url = await _transactionService.CreatePayment(createPaymentRequest);
			return Ok(url);
		}

		[HttpGet(ApiEndPointConstant.Payment.PaymentEndpoint)]
		public async Task<IActionResult> PaymentCallBack(string? vnp_Amount, string? vnp_BankCode,
			string? vnp_BankTranNo, string? vnp_CardType, string? vnp_OrderInfo, string? vnp_PayDate,
			string? vnp_ResponseCode, string? vnp_TmnCode, string? vnp_TransactionNo, string? vnp_TxnRef,
			string? vnp_SecureHashType, string? vnp_SecureHash)
		{
			//bool isSuccessful = await _transactionService.PaymentExecute(vnp_Amount, vnp_BankCode, vnp_BankTranNo,
			//	vnp_CardType, vnp_OrderInfo, vnp_PayDate, vnp_ResponseCode, vnp_TmnCode, vnp_TransactionNo, vnp_TxnRef,
			//	vnp_SecureHashType, vnp_SecureHash);
			//return Ok(isSuccessful);
			//_transactionService.CreateMapping();
			return Ok();
		}
	}
}
