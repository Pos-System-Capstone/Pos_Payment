using ResoPayment.ApplicationCore.Interfaces;
using ResoPayment.Enums;
using ResoPayment.Extensions;
using ResoPayment.Infrastructure;
using ResoPayment.Infrastructure.Models;
using ResoPayment.Payload.Request;
using ResoPayment.Payload.Response;
using ResoPayment.Service.Interfaces;

namespace ResoPayment.Service.Implements
{
	public class VnPayService : IVnPayServices
	{
		private readonly IConfiguration _configuration;

		public VnPayService(IUnitOfWork<PosPaymentContext> unitOfWork, ILogger<VnPayService> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
		{
			_configuration = configuration;
		}
		//public async Task<string> CreatePaymentUrl(PaymentInformationRequest model, HttpContext context)
		//{
		//	Guid storeId = Guid.Parse(GetStoreIdFromJwt());
		//	var store = await _unitOfWork.GetRepository<Store>().SingleOrDefaultAsync(selector: x => new {StoreId = x.Id, BrandId = x.BrandId},predicate: x => x.Id.Equals(storeId));
		//	var newOrder = new Order()
		//	{
		//		Id = model.OrderId,
		//		CheckOutDate = DateTime.UtcNow,
		//		TotalAmount = model.Amount,
		//		DiscountAmount = 0,
		//		FinalAmount = model.Amount,
		//		Vatamount = 0,
		//		Vat = 0,
		//		InvoiceId = model.InvoiceId,
		//		StoreId = store.StoreId,
		//	};
		//	var newTransaction = new Transaction()
		//	{
		//		Id = Guid.NewGuid(),
		//		BrandId = store.BrandId,
		//		StoreId = store.StoreId,
		//		Status = TransactionStatus.Pending.ToString(),
		//		Amount = model.Amount,
		//		AccountId = Guid.NewGuid(),
		//		IsIncreaseTransaction = true,
		//		PaymentProviderId = Guid.Parse("4C6AEFA8-9FCF-4E46-9370-BEBDEF6EA55C"), //VNPAY ID
		//		TransactionCode = String.Empty,
		//		FcAmount = 0,
		//		CurrencyCode = _configuration["Vnpay:CurrCode"],
		//		Notes = model.OrderDescription,
		//		OrderId = newOrder.Id
		//	};
		//	newOrder.Transaction = newTransaction;
		//	_unitOfWork.GetRepository<Order>().InsertAsync(newOrder);
		//	bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
		//	var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
		//	var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
		//	var tick = DateTime.Now.Ticks.ToString();
		//	var pay = new VnPayLibrary();
		//	var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

		//	pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
		//	pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
		//	pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
		//	pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
		//	pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
		//	pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
		//	pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
		//	pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
		//	pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription} {model.Amount}");
		//	pay.AddRequestData("vnp_OrderType", model.OrderType);
		//	pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
		//	pay.AddRequestData("vnp_TxnRef", model.OrderId.ToString());

		//	var paymentUrl = pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);
		//	return paymentUrl;
		//}

		//public PaymentResponseModel PaymentExecute(IQueryCollection collections)
		//{
		//	var pay = new VnPayLibrary();
		//	var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);

		//	return response;
		//}
	}
}
