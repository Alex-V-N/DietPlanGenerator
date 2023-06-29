using AdminPanel.ActionFilters;
using AdminPanel.Models.Users;
using AdminPanel.RestComunication.FitCookieAI;
using AdminPanel.RestComunication.FitCookieAI.Responses.Admins;
using AdminPanel.RestComunication.FitCookieAI.Responses.AdminStatuses;
using AdminPanel.RestComunication.FitCookieAI.Responses.Users;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
	[AdminsAuthenticationFilter]
	public class UsersController : Controller
	{
        private readonly ILogger<UsersController> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        GetAllUsersResponse _getAllUsersResponse;

        private FitCookieAI_RequestBuilder _fitCookieAIRequestBuilder;
        private FitCookieAI_RequestExecutor _fitCookieAIRequestExecutor;

        string baseFitcookieAIUri;

        public UsersController(IWebHostEnvironment hostEnvironment, ILogger<UsersController> logger,
           IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            _fitCookieAIRequestBuilder = new FitCookieAI_RequestBuilder();
            _fitCookieAIRequestExecutor = new FitCookieAI_RequestExecutor(_httpContextAccessor);

            _getAllUsersResponse = new GetAllUsersResponse();

            webHostEnvironment = hostEnvironment;
            _logger = logger;

            baseFitcookieAIUri = _configuration.GetValue<string>("FitCookieAI_API");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
		{
            UsersIndexVM model = new UsersIndexVM();
            _getAllUsersResponse = await _fitCookieAIRequestExecutor.GetAllUsersAction(_fitCookieAIRequestBuilder.GetAllUsersRequestBuilder(baseFitcookieAIUri));

            if (_getAllUsersResponse.Code != null && int.Parse(_getAllUsersResponse.Code.ToString()) == 201)
            {
                model.Users = _getAllUsersResponse.Body;
            }

			return View(model);
		}
	}
}
