using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResoPayment.Constants;

namespace ResoPayment.Controllers
{
	[ApiController]
	public class OrdersController : BaseController<OrdersController>
	{
		public OrdersController(ILogger<OrdersController> logger) : base(logger)
		{
		}

		[Authorize]
		[HttpGet(ApiEndPointConstant.Order.OrderEndpoint)]
		public async Task<IActionResult> CreateNewPayment()
		{
			return Ok("Tao thanh cong");
		}
	}
}
