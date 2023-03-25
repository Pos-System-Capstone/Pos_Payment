using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ResoPayment.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{

		[Authorize]
		[HttpGet]
		public IActionResult HelloWorld()
		{
			return Ok("Hello world");
		}

	}
}
