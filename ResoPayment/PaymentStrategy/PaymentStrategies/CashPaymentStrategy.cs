using Microsoft.Extensions.Caching.Distributed;
using ResoPayment.ApplicationCore.Interfaces;
using ResoPayment.Enums;
using ResoPayment.Helpers;
using ResoPayment.Infrastructure;
using ResoPayment.Infrastructure.Models;
using ResoPayment.Payload.Response;
using ResoPayment.RedisModels;
using ResoPayment.Service.Interfaces;

namespace ResoPayment.PaymentStrategy.PaymentStrategies;

public class CashPaymentStrategy : IPaymentStrategy
{
	private readonly IUnitOfWork<PosPaymentContext> _unitOfWork;
	private readonly Transaction _transaction;
	private readonly IDistributedCache _distributedCache;
	public CashPaymentStrategy(Transaction transaction, IUnitOfWork<PosPaymentContext> unitOfWork, IDistributedCache distributedCache)
	{
		_transaction = transaction;
		_unitOfWork = unitOfWork;
		_distributedCache = distributedCache;
	}

	public async Task<CreatePaymentResponse> ExecutePayment()
	{
		if (_transaction == null) throw new BadHttpRequestException("Không tìm thấy giao dịch");
		_transaction.Status = TransactionStatus.Paid.ToString();
		_unitOfWork.GetRepository<Transaction>().UpdateAsync(_transaction);
		OrderData orderData;
		byte[] orderDataRedis;
		DistributedCacheEntryOptions redisEntryOption;
		bool isSuccessful =  await _unitOfWork.CommitAsync() > 0;
		if (isSuccessful)
		{
			orderData = new OrderData()
			{
				Id = _transaction.OrderId,
				TransactionStatus = TransactionStatus.Paid
			};
			orderDataRedis = RedisHelper.EncodeOrderData(orderData);
			redisEntryOption = RedisHelper.SetUpRedisEntryOptions();
			await _distributedCache.SetAsync(_transaction.OrderId.ToString(), orderDataRedis, redisEntryOption);
			return new CreatePaymentResponse()
			{
				Url = null,
				Message = "Thanh toán thành công",
				DisplayType = CreatePaymentReturnType.Message
			};
		}
		orderData = new OrderData()
		{
			Id = _transaction.OrderId,
			TransactionStatus = TransactionStatus.Fail
		};
		orderDataRedis = RedisHelper.EncodeOrderData(orderData);
		redisEntryOption = RedisHelper.SetUpRedisEntryOptions();
		await _distributedCache.SetAsync(_transaction.OrderId.ToString(), orderDataRedis, redisEntryOption);
		return new CreatePaymentResponse()
		{
			Url = null,
			Message = "Thanh toán thất bại",
			DisplayType = CreatePaymentReturnType.Message
		};
	}
}