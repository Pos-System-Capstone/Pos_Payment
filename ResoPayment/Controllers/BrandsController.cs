using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResoPayment.Constants;
using ResoPayment.Payload.Request;
using ResoPayment.Service.Interfaces;

namespace ResoPayment.Controllers
{
	[ApiController]
	public class BrandsController : BaseController<BrandsController>
	{
		private readonly IBrandService _brandService;
		public BrandsController(ILogger<BrandsController> logger, IBrandService brandService) : base(logger)
		{
			_brandService = brandService;
		}

		[Authorize]
		[HttpPost(ApiEndPointConstant.Brand.CreateBrandPaymentProviderMappingEndPoint)]
		public async Task<IActionResult> CreateBrandPaymentProviderMapping(
			[FromBody] CreateBrandPaymentProviderMappingRequest request)
		{
			bool isSuccessfully = await _brandService.CreateBrandPaymentProviderMapping(request);
			if (isSuccessfully)
			{
				return Ok("Tạo phương thức thanh toán thành công");
			}

			return Ok("Tạo phương thức thanh toán thất bại");
		}
	}
}
