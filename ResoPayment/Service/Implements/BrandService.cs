using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ResoPayment.ApplicationCore.Interfaces;
using ResoPayment.Helpers;
using ResoPayment.Infrastructure.Models;
using ResoPayment.Infrastructure.PaymentConfigModels;
using ResoPayment.Payload.Request;
using ResoPayment.Payload.Response;
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
				CreateDate = DateTimeHelper.ConvertDateTimeToVietNamTimeZone()
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
		else
		{
			IEnumerable<BrandPaymentProviderMapping> brandPaymentProviderMappings = await _unitOfWork
				.GetRepository<BrandPaymentProviderMapping>().GetListAsync(predicate: x => x.BrandId.Equals(brand.Id), include: x => x.Include(x => x.PaymentProvider));
			if(!brandPaymentProviderMappings.Any(x => x.PaymentProvider.Type.ToUpper().Equals("VIETQR")) && request.VietQrConfigRequest != null)
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
					BrandId = brand.Id,
					Status = "Active",
					PaymentProviderId = vietQrPaymentProvider.Id,
					Config = vietQrConfigSerialize
				};
				await _unitOfWork.GetRepository<BrandPaymentProviderMapping>()
					.InsertAsync(newVietQrBrandPaymentProviderMapping);
			}
			if (!brandPaymentProviderMappings.Any(x => x.PaymentProvider.Type.ToUpper().Equals("ZALOPAY")) && request.ZaloPayConfigRequest != null)
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
					BrandId = brand.Id,
					Status = "Active",
					PaymentProviderId = zaloPayPaymentProvider.Id,
					Config = zaloPayConfigSerialize
				};
				await _unitOfWork.GetRepository<BrandPaymentProviderMapping>()
					.InsertAsync(newZaloPayBrandPaymentProviderMapping);
			}

			if (!brandPaymentProviderMappings.Any(x => x.PaymentProvider.Type.ToUpper().Equals("VNPAY")) && request.VnPayConfigRequest != null)
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
					BrandId = brand.Id,
					Status = "Active",
					PaymentProviderId = vnPayPaymentProvider.Id,
					Config = vnPayConfigSerialize
				};
				await _unitOfWork.GetRepository<BrandPaymentProviderMapping>()
					.InsertAsync(newVnPayBrandPaymentProviderMapping);
			}
			
			foreach (var brandPaymentProviderMapping in brandPaymentProviderMappings)
			{
				if (brandPaymentProviderMapping.PaymentProvider.Type.ToUpper().Equals("VIETQR") && request.VietQrConfigRequest != null)
				{
					VietQRConfig vietQrConfig = new VietQRConfig()
					{
						BankCode = request.VietQrConfigRequest.BankCode,
						AccountNo = request.VietQrConfigRequest.AccountNumber,
						AccountName = request.VietQrConfigRequest.AccountName
					};
					string vietQrConfigSerialize = JsonConvert.SerializeObject(vietQrConfig);
					brandPaymentProviderMapping.Config = vietQrConfigSerialize;
					_unitOfWork.GetRepository<BrandPaymentProviderMapping>().UpdateAsync(brandPaymentProviderMapping);
				}

				if (brandPaymentProviderMapping.PaymentProvider.Type.ToUpper().Equals("ZALOPAY") && request.ZaloPayConfigRequest != null)
				{
					ZaloPayConfig zaloPayConfig = new ZaloPayConfig()
					{
						AppId = request.ZaloPayConfigRequest.AppId,
						BankCode = "zalopayapp",
						Key2 = request.ZaloPayConfigRequest.Key2,
						Key1 = request.ZaloPayConfigRequest.Key1,
						BaseUrl = "https://sandbox.zalopay.com.vn/v001/tpe/createorder"
					};
					string zaloPayConfigSerialize = JsonConvert.SerializeObject(zaloPayConfig);
					brandPaymentProviderMapping.Config = zaloPayConfigSerialize;
					_unitOfWork.GetRepository<BrandPaymentProviderMapping>().UpdateAsync(brandPaymentProviderMapping);
				}

				if (brandPaymentProviderMapping.PaymentProvider.Type.ToUpper().Equals("VNPAY") && request.VnPayConfigRequest != null)
				{
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
					brandPaymentProviderMapping.Config = vnPayConfigSerialize;
					_unitOfWork.GetRepository<BrandPaymentProviderMapping>().UpdateAsync(brandPaymentProviderMapping);
				}
			}

			return await _unitOfWork.CommitAsync() > 0;
		}
	}

	public async Task<GetBrandPaymentProviderMappingResponse> GetBrandPaymentProviderMapping(Guid brandId)
	{
		Brand brand = await _unitOfWork.GetRepository<Brand>()
			.SingleOrDefaultAsync(predicate: x => x.Id.Equals(brandId));
		if (brand == null) throw new BadHttpRequestException("Không tìm thấy brand");
		GetBrandPaymentProviderMappingResponse getBrandPaymentProviderMappingResponse =
			new GetBrandPaymentProviderMappingResponse()
			{
				BrandId = brand.Id,
				BrandName = brand.Name,
				BrandPhoneNumber = brand.PhoneNumber,
			};
		IEnumerable<BrandPaymentProviderMapping> brandPaymentProviderMappings = await _unitOfWork
			.GetRepository<BrandPaymentProviderMapping>().GetListAsync(predicate: x => x.BrandId.Equals(brand.Id), include: x => x.Include(x => x.PaymentProvider));
		foreach (var brandPaymentProviderMapping in brandPaymentProviderMappings)
		{
			if (brandPaymentProviderMapping.PaymentProvider.Type.ToUpper().Equals("VIETQR"))
			{
				VietQRConfig vietQrConfig =
					JsonConvert.DeserializeObject<VietQRConfig>(brandPaymentProviderMapping.Config);
				getBrandPaymentProviderMappingResponse.VietQrConfigRequest = new VietQRConfigRequest()
				{
					BankCode = vietQrConfig.BankCode,
					AccountName = vietQrConfig.AccountName,
					AccountNumber = vietQrConfig.AccountNo
				};
			}

			if (brandPaymentProviderMapping.PaymentProvider.Type.ToUpper().Equals("ZALOPAY"))
			{
				ZaloPayConfig zaloPayConfig = JsonConvert.DeserializeObject<ZaloPayConfig>(brandPaymentProviderMapping.Config);
				getBrandPaymentProviderMappingResponse.ZaloPayConfigRequest = new ZaloPayConfigRequest()
				{
					AppId = zaloPayConfig.AppId,
					Key1 = zaloPayConfig.Key1,
					Key2 = zaloPayConfig.Key2
				};
			}

			if (brandPaymentProviderMapping.PaymentProvider.Type.ToUpper().Equals("VNPAY"))
			{
				VnPayConfig vnPayConfig =
					JsonConvert.DeserializeObject<VnPayConfig>(brandPaymentProviderMapping.Config);
				getBrandPaymentProviderMappingResponse.VnPayConfigRequest = new VnPayConfigRequest()
				{
					SecureHash = vnPayConfig.SecureHash,
					TmnCode = vnPayConfig.TmnCode
				};
			}
		}
		return getBrandPaymentProviderMappingResponse;
	}
}