using Newtonsoft.Json;
using ResoPayment.ApplicationCore.Interfaces;
using ResoPayment.Infrastructure.Models;
using ResoPayment.Infrastructure.PaymentConfigModels;
using ResoPayment.Payload.Request;
using ResoPayment.Service.Interfaces;

namespace ResoPayment.Service.Implements;

public class BrandService : BaseService<BrandService>, IBrandService
{
	public BrandService(IUnitOfWork<PosPaymentContext> unitOfWork, ILogger<BrandService> logger, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(unitOfWork, logger, httpContextAccessor, configuration)
	{
	}

	public async Task<bool> CreateBrandPaymentProviderMapping(CreateBrandPaymentProviderMappingRequest request)
	{
		Brand brand = await _unitOfWork.GetRepository<Brand>()
			.SingleOrDefaultAsync(predicate: x => x.Id.Equals(request.BrandId));
		if (brand == null)
		{
			Brand newBrand = new Brand()
			{
				Id = request.BrandId,
				Name = request.BrandName,
				PhoneNumber = request.BrandPhoneNumber,
				Status = "Active",
				CreateDate = DateTime.UtcNow
			};
			newBrand.Stores = new List<Store>();
			request.CreateStoreRequests.ForEach(store =>
			{
				Store newStore = new Store()
				{
					Id = store.StoreId,
					Name = store.StoreName,
					Address = store.StoreAddress,
					PhoneNumber = store.StorePhoneNumber,
					Email = store.StoreEmail,
					BrandId = newBrand.Id
				};
				newBrand.Stores.Add(newStore);
			});
			newBrand.BrandPaymentProviderMappings = new List<BrandPaymentProviderMapping>();
			if (request.VietQrConfigRequest != null)
			{
				PaymentProvider vietQrPaymentProvider = await _unitOfWork.GetRepository<PaymentProvider>()
					.SingleOrDefaultAsync(predicate: x => x.Type.ToUpper().Equals("VIETQR"));
				VietQRConfig vietQrConfig = new VietQRConfig()
				{
					BankCode = request.VietQrConfigRequest.BankCode,
					AccountNo = request.VietQrConfigRequest.AccountNumber,
					AccountName = request.VietQrConfigRequest.AccountName
				};
				string vietQrConfigSerialize = JsonConvert.SerializeObject(vietQrConfig);
				BrandPaymentProviderMapping newVietQrBrandPaymentProviderMapping = new BrandPaymentProviderMapping()
				{
					Id = Guid.NewGuid(),
					BrandId = newBrand.Id,
					Status = "Active",
					PaymentProviderId = vietQrPaymentProvider.Id,
					Config = vietQrConfigSerialize
				};
				newBrand.BrandPaymentProviderMappings.Add(newVietQrBrandPaymentProviderMapping);
			}

			if (request.VnPayConfigRequest != null)
			{
				PaymentProvider vnPayPaymentProvider = await _unitOfWork.GetRepository<PaymentProvider>()
					.SingleOrDefaultAsync(predicate: x => x.Type.ToUpper().Equals("VNPAY"));
				VnPayConfig vnPayConfig = new VnPayConfig()
				{
					BaseUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
					SecureHash = request.VnPayConfigRequest.SecureHash,
					TmnCode = request.VnPayConfigRequest.TmnCode,
					Version = "2.1.0",
					Command = "pay",
					CurrCode = "VND"
				};
				string vnPayConfigSerialize = JsonConvert.SerializeObject(vnPayConfig);
				BrandPaymentProviderMapping newVnPayBrandPaymentProviderMapping = new BrandPaymentProviderMapping()
				{
					Id = Guid.NewGuid(),
					BrandId = newBrand.Id,
					Status = "Active",
					PaymentProviderId = vnPayPaymentProvider.Id,
					Config = vnPayConfigSerialize
				};
				newBrand.BrandPaymentProviderMappings.Add(newVnPayBrandPaymentProviderMapping);
			}

			if (request.ZaloPayConfigRequest != null)
			{
				PaymentProvider zaloPayPaymentProvider = await _unitOfWork.GetRepository<PaymentProvider>()
					.SingleOrDefaultAsync(predicate: x => x.Type.ToUpper().Equals("ZALOPAY"));
				ZaloPayConfig zaloPayConfig = new ZaloPayConfig()
				{
					AppId = request.ZaloPayConfigRequest.AppId,
					BankCode = "zalopayapp",
					Key2 = request.ZaloPayConfigRequest.Key2,
					Key1 = request.ZaloPayConfigRequest.Key1,
					BaseUrl = "https://sandbox.zalopay.com.vn/v001/tpe/createorder"
				};
				string zaloPayConfigSerialize = JsonConvert.SerializeObject(zaloPayConfig);
				BrandPaymentProviderMapping newZaloPayBrandPaymentProviderMapping = new BrandPaymentProviderMapping()
				{
					Id = Guid.NewGuid(),
					BrandId = newBrand.Id,
					Status = "Active",
					PaymentProviderId = zaloPayPaymentProvider.Id,
					Config = zaloPayConfigSerialize
				};
				newBrand.BrandPaymentProviderMappings.Add(newZaloPayBrandPaymentProviderMapping);
			}

			PaymentProvider cash = await _unitOfWork.GetRepository<PaymentProvider>()
				.SingleOrDefaultAsync(predicate: x => x.Type.ToUpper().Equals("CASH"));
			BrandPaymentProviderMapping cashBrandPaymentProviderMapping = new BrandPaymentProviderMapping()
			{
				Id = Guid.NewGuid(),
				Status = "Active",
				BrandId = newBrand.Id,
				PaymentProviderId = cash.Id
			};
			newBrand.BrandPaymentProviderMappings.Add(cashBrandPaymentProviderMapping);
			await _unitOfWork.GetRepository<Brand>().InsertAsync(newBrand);
			return await _unitOfWork.CommitAsync() > 0;
		}

		return false;
	}
}