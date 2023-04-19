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
	private readonly Guid _transactionId;
	public CashPaymentStrategy( Guid transactionId, IUnitOfWork<PosPaymentContext> unitOfWork)
	{
		_transactionId = transactionId;
		_unitOfWork = unitOfWork;
	}

	public async Task<CreatePaymentResponse> ExecutePayment()
	{
		var transaction = await _unitOfWork.GetRepository<Transaction>()
			.SingleOrDefaultAsync(predicate: x => x.Id.Equals(_transactionId));
		if (transaction == null) throw new BadHttpRequestException("Không tìm thấy giao dịch");
		transaction.Status = TransactionStatus.Paid.ToString();
		_unitOfWork.GetRepository<Transaction>().UpdateAsync(transaction);
		bool isSuccessful =  await _unitOfWork.CommitAsync() > 1;
		if (isSuccessful)
		{
			return new CreatePaymentResponse()
			{
				Url = "",
				Message = "Giao dịch thành công",
				DisplayType = CreatePaymentReturnType.Message
			};
		}

		return new CreatePaymentResponse()
		{
			Url = "",
			Message = "Giao dịch thất bại",
			DisplayType = CreatePaymentReturnType.Message
		};
	}
}