using System.Security.Claims;
using ResoPayment.ApplicationCore.Interfaces;
using ResoPayment.Infrastructure;
using ResoPayment.Infrastructure.Models;
using ResoPayment.Service.Implements;

namespace ResoPayment.Service;

public abstract class BaseService<T> where T : class
{
	protected IUnitOfWork<PosPaymentContext> _unitOfWork;
	protected ILogger<T> _logger;
	protected IHttpContextAccessor _httpContextAccessor;
    private IUnitOfWork<PosPaymentContext> unitOfWork;
    private ILogger<VnPayService> logger;
    private IHttpContextAccessor httpContextAccessor;

    public BaseService(IUnitOfWork<PosPaymentContext> unitOfWork, ILogger<T> logger, IHttpContextAccessor httpContextAccessor)
	{
		_unitOfWork = unitOfWork;
		_logger = logger;
		_httpContextAccessor = httpContextAccessor;
	}

    protected BaseService(IUnitOfWork<PosPaymentContext> unitOfWork, ILogger<VnPayService> logger, IHttpContextAccessor httpContextAccessor)
    {
        this.unitOfWork = unitOfWork;
        this.logger = logger;
        this.httpContextAccessor = httpContextAccessor;
    }

    protected string GetStoreIdFromJwt()
	{
		return _httpContextAccessor?.HttpContext?.User?.FindFirstValue("storeId");
	}
}