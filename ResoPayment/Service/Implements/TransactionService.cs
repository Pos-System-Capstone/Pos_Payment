using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ResoPayment.ApplicationCore.Interfaces;
using ResoPayment.Constants;
using ResoPayment.Enums;
using ResoPayment.Helpers;
using ResoPayment.Infrastructure;
using ResoPayment.Infrastructure.Models;
using ResoPayment.Payload.Request;
using ResoPayment.Payload.Response;
using ResoPayment.PaymentStrategy;
using ResoPayment.PaymentStrategy.PaymentStrategies;
using ResoPayment.RedisModels;
using ResoPayment.Service.Interfaces;

namespace ResoPayment.Service.Implements;

public class TransactionService : BaseService<TransactionService>, ITransactionService
{
    private readonly IDistributedCache _distributedCache;
    public TransactionService(IUnitOfWork<PosPaymentContext> unitOfWork, ILogger<TransactionService> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IDistributedCache distributedCache) : base(unitOfWork, logger, httpContextAccessor, configuration)
    {
	    _distributedCache = distributedCache;
    }

    //public async Task<bool> PaymentExecute(string? vnp_Amount, string? vnp_BankCode, string? vnp_BankTranNo,
    //	string? vnp_CardType, string? vnp_OrderInfo, string? vnp_PayDate, string? vnp_ResponseCode, string? vnp_TmnCode,
    //	string? vnp_TransactionNo, string? vnp_TxnRef, string? vnp_SecureHashType, string? vnp_SecureHash)
    //{
    //	//Get order
    //	vnp_TxnRef = vnp_TxnRef.Trim();
    //	var orderId = Guid.Parse(vnp_TxnRef);
    //	var Order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(orderId));
    //	var transaction = await _unitOfWork.GetRepository<Transaction>().SingleOrDefaultAsync(predicate:x => x.OrderId.Equals(orderId));
    //	if (vnp_ResponseCode.Equals("00"))
    //	{
    //		transaction.Status = TransactionStatus.Paid.ToString();
    //		transaction.TransactionCode = vnp_TransactionNo;
    //	}
    //	else
    //	{
    //		transaction.Status = TransactionStatus.Fail.ToString();
    //		transaction.TransactionCode = vnp_TransactionNo;
    //	}
    //	_unitOfWork.GetRepository<Transaction>().UpdateAsync(transaction);
    //	bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
    //	return isSuccessful;
    //}

    public async Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest createPaymentRequest)
    {
        var store = await _unitOfWork.GetRepository<Store>()
            .SingleOrDefaultAsync(predicate: x => x.Id.Equals(createPaymentRequest.StoreId), include: x => x.Include(x => x.Brand));
        var paymentProvider = await _unitOfWork.GetRepository<PaymentProvider>()
            .SingleOrDefaultAsync(predicate: x => x.Id.Equals(createPaymentRequest.PaymentId));
        if (store == null) throw new BadHttpRequestException("Không tìm thấy cửa hàng");
        if (paymentProvider == null) throw new BadHttpRequestException("Không tìm thấy payment provider");
        var newOrder = new Order()
        {
            Id = createPaymentRequest.OrderId,
            InvoiceId = createPaymentRequest.InvoiceId,
            StoreId = store.Id,
            TotalAmount = createPaymentRequest.Amount,
            CheckOutDate = DateTime.UtcNow,
        };
        newOrder.Transaction = new Transaction()
        {
            Id = Guid.NewGuid(),
            StoreId = store.Id,
            BrandId = store.BrandId,
            OrderId = newOrder.Id,
            AccountId = createPaymentRequest.AccountId,
            Amount = createPaymentRequest.Amount,
            Status = TransactionStatus.Pending.ToString(),
            CurrencyCode = "VND",
            Notes = createPaymentRequest.OrderDescription,
            PaymentProviderId = paymentProvider.Id,
            TransactionCode = String.Empty
        };
        await _unitOfWork.GetRepository<Order>().InsertAsync(newOrder);
        bool isSuccessful = await _unitOfWork.CommitAsync() > 0;
        IPaymentStrategy paymentStrategy;
        if (isSuccessful)
        {
            var brandPaymentConfig = await _unitOfWork.GetRepository<BrandPaymentProviderMapping>()
                .SingleOrDefaultAsync(selector: x => x.Config,
                    predicate: x =>
                    x.BrandId.Equals(store.BrandId) && x.PaymentProviderId.Equals(paymentProvider.Id));
            switch (paymentProvider.Name)
            {
                case PaymentProviderConstant.VNPAY:
                    paymentStrategy = new VnPayPaymentStrategy(brandPaymentConfig, _httpContextAccessor.HttpContext,
                        newOrder.Id, createPaymentRequest.OrderDescription, newOrder.TotalAmount,
                        _configuration["PaymentCallBack:ReturnUrl"], _configuration["Vnpay:HashSecret"]);
                    return await paymentStrategy.ExecutePayment();
                    break;
                case PaymentProviderConstant.ZALOPAY:
                    paymentStrategy = new ZaloPayPaymentStrategy(brandPaymentConfig, newOrder.Id, createPaymentRequest.OrderDescription, newOrder.TotalAmount, _httpContextAccessor);
                    return await paymentStrategy.ExecutePayment();
                case PaymentProviderConstant.VIETQR:
                    paymentStrategy = new VietQRPaymentStrategy(brandPaymentConfig, createPaymentRequest.OrderDescription, newOrder.TotalAmount);
                    return await paymentStrategy.ExecutePayment();
                default:
                    throw new BadHttpRequestException("Không tìm thấy payment provider");
            }
        }
        else
        {
            throw new BadHttpRequestException("Tạo mới giao dịch thất bại");
        }

    }

    public async Task<bool> ExecuteZaloPayCallBack(double? amount, double? discountamount, string? appid, string? checksum, string? apptransid,
	    int? status)
    {
	    var orderId = apptransid.Substring(apptransid.IndexOf("_") + 1);
	    var order = await _unitOfWork.GetRepository<Order>().SingleOrDefaultAsync(predicate: x => x.Id.Equals(Guid.Parse(orderId)));
	    var transaction = await _unitOfWork.GetRepository<Transaction>()
		    .SingleOrDefaultAsync(predicate: x => x.OrderId.Equals(order.Id));
	    if (order == null || transaction == null)
		    throw new BadHttpRequestException("Không tìm thấy order hoặc transaction cho order");
	    if (status != 1)
	    {
		    OrderData orderData = new OrderData()
		    {
			    Id = order.Id,
			    TransactionStatus = TransactionStatus.Fail
		    };
		    var orderDataRedis = RedisHelper.EncodeOrderData(orderData);
		    var redisEntryOption = RedisHelper.SetUpRedisEntryOptions();
		    await _distributedCache.SetAsync(order.Id.ToString(), orderDataRedis, redisEntryOption);
            transaction.Status = TransactionStatus.Fail.ToString();
	    }
	    else
	    {
		    OrderData orderData = new OrderData()
		    {
			    Id = order.Id,
			    TransactionStatus = TransactionStatus.Paid
		    };
		    var orderDataRedis = RedisHelper.EncodeOrderData(orderData);
		    var redisEntryOption = RedisHelper.SetUpRedisEntryOptions();
		    await _distributedCache.SetAsync(order.Id.ToString(), orderDataRedis, redisEntryOption);
		    transaction.Status = TransactionStatus.Paid.ToString();
	    }
	    _unitOfWork.GetRepository<Transaction>().UpdateAsync(transaction);
	    return await _unitOfWork.CommitAsync() > 0;
    }

    public async Task<OrderData> CheckTransactionStatus(string orderid)
    {
	    byte[] orderDataBytes = await _distributedCache.GetAsync(orderid);
	    OrderData orderData = null;
	    if (orderDataBytes != null)
	    {
            string serilizedOrderData = Encoding.UTF8.GetString(orderDataBytes);
            orderData = JsonConvert.DeserializeObject<OrderData>(serilizedOrderData);
	    }
        return orderData;
    }
}