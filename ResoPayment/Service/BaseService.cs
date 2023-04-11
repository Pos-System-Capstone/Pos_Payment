using System.Security.Claims;
using ResoPayment.ApplicationCore.Interfaces;
using ResoPayment.Infrastructure;
using ResoPayment.Infrastructure.Models;

namespace ResoPayment.Service;

public abstract class BaseService<T> where T : class
{
	protected IUnitOfWork<PosPaymentContext> _unitOfWork;
	protected ILogger<T> _logger;
	protected IHttpContextAccessor _httpContextAccessor;
	protected IConfiguration _configuration;

	public BaseService(IUnitOfWork<PosPaymentContext> unitOfWork, ILogger<T> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_httpContextAccessor = httpContextAccessor;
		_configuration = configuration;
	}
	protected string GetStoreIdFromJwt()
	{
		return _httpContextAccessor?.HttpContext?.User?.FindFirstValue("storeId");
	}
}