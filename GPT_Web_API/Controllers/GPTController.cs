using GPT_Web_API.Requests;
using GPT_Web_API.Responses;
using GPT_Web_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GPT_Web_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GPTController : ControllerBase
	{
		private readonly ILogger<GPTController> _logger;
		private readonly IConfiguration _config;
		private GPT_Service _service;

		public GPTController(ILogger<GPTController> logger, IConfiguration config, GPT_Service service)
		{
			_logger = logger;
			_config = config;
			_service = service;
		}

		[HttpGet]
		[Route("Get")]
		public async Task<IActionResult> GetGPTResponse(string input)
		{
			if (!string.IsNullOrEmpty(input))
			{
				BaseGPTRequest request = new BaseGPTRequest();
				BaseGPTResponse response = new BaseGPTResponse();

				request.model = _config.GetValue<string>("OpenAI_API_Request_Model");
				request.prompt = input;
				request.max_tokens = 3000;
				request.temperature = 0;

				using (var httpClient = new HttpClient())
				{
					response = await _service.GetGPTResponse(httpClient, request);
				}

				if (response != null)
				{
					return new JsonResult(response);
					//data = leaveTypes
				}
				else
				{
					return new JsonResult(500, "GPT returned null!");
				}
			}
			else
			{
				return new JsonResult(400, "Input cannot be null or empty!");
			}
		}
	}
}
