using ResoPayment.ApplicationCore.Interfaces;
using ResoPayment.Enums;
using ResoPayment.Infrastructure;
using ResoPayment.Infrastructure.Models;
using ResoPayment.Payload.Response;
using ResoPayment.Service.Interfaces;

namespace ResoPayment.PaymentStrategy.PaymentStrategies;

public class CashPaymentStrategy : IPaymentStrategy
{
	private readonly IUnitOfWork<PosPaymentContext> _unitOfWork;
	private readonly Transaction _transaction;
	public CashPaymentStrategy( Transaction transaction, IUnitOfWork<PosPaymentContext> unitOfWork)
	{
		_transaction = transaction;
		_unitOfWork = unitOfWork;
	}

	public async Task<CreatePaymentResponse> ExecutePayment()
	{
		if (_transaction == null) throw new BadHttpRequestException("Không tìm thấy giao dịch");
		_transaction.Status = TransactionStatus.Paid.ToString();
		_unitOfWork.GetRepository<Transaction>().UpdateAsync(_transaction);
		bool isSuccessful =  await _unitOfWork.CommitAsync() > 0;
		if (isSuccessful)
		{
			return new CreatePaymentResponse()
			{
				Url = null,
				Message = "Thanh toán thành công",
				DisplayType = CreatePaymentReturnType.Message
			};
		}

		return new CreatePaymentResponse()
		{
			Url = null,
			Message = "Thanh toán thất bại",
			DisplayType = CreatePaymentReturnType.Message
		};
	}
}