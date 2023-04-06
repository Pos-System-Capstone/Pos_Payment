using ResoPayment.ApplicationCore.Interfaces;
using ResoPayment.Infrastructure.Models;

namespace ResoPayment.Service;

public abstract class BaseService<T> where T : class
{
	protected IUnitOfWork<PosPaymentContext> _unitOfWork;
	protected ILogger<T> _logger;

	public BaseService(IUnitOfWork<PosPaymentContext> unitOfWork, ILogger<T> logger)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
	}
}