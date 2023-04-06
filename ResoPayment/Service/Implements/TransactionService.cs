using ResoPayment.ApplicationCore.Interfaces;
using ResoPayment.Infrastructure.Models;

namespace ResoPayment.Service.Implements;

public class TransactionService : BaseService<TransactionService>
{
	public TransactionService(IUnitOfWork<PosPaymentContext> unitOfWork, ILogger<TransactionService> logger) : base(unitOfWork, logger)
	{
	}
}