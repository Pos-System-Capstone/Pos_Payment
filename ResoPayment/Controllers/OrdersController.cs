using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResoPayment.Constants;
using ResoPayment.Payload.Request;
using ResoPayment.Service.Interfaces;

namespace ResoPayment.Controllers
{
    [ApiController]
    public class OrdersController : BaseController<OrdersController>
    {

        private readonly IVnPayServices _vnPayService;
        public OrdersController(ILogger<OrdersController> logger, IVnPayServices vnPayService) : base(logger)
        {
            _vnPayService = vnPayService;
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.Order.OrderEndpoint)]
        public async Task<IActionResult> CreateNewPayment()
        {
            return Ok("Tao thanh cong");
        }
    }
}
