using AdminPanel.ActionFilters;
using AdminPanel.Models.Admins;
using AdminPanel.Models.AdminStatuses;
using AdminPanel.RestComunication.FitCookieAI;
using AdminPanel.RestComunication.FitCookieAI.Responses.AdminStatuses;
using AdminPanel.RestComunication.FitCookieAI.Responses.Users;
using FitCookieAI_API.Controllers.AdminRelated;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    [AdminsAuthenticationFilter]
    public class AdminStatusesController : Controller
    {
        private readonly ILogger<AdminStatusesController> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private GetAllAdminStatusesResponse _getAllAdminStatusesResponse;

        private FitCookieAI_RequestBuilder _fitCookieAIRequestBuilder;
        private FitCookieAI_RequestExecutor _fitCookieAIRequestExecutor;

        string baseFitcookieAIUri;

        public AdminStatusesController(IWebHostEnvironment hostEnvironment, ILogger<AdminStatusesController> logger,
           IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            _fitCookieAIRequestBuilder = new FitCookieAI_RequestBuilder();
            _fitCookieAIRequestExecutor = new FitCookieAI_RequestExecutor(_httpContextAccessor);

            _getAllAdminStatusesResponse = new GetAllAdminStatusesResponse();

            webHostEnvironment = hostEnvironment;
            _logger = logger;

            baseFitcookieAIUri = _configuration.GetValue<string>("FitCookieAI_API");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AdminStatusesIndexVM model = new AdminStatusesIndexVM();
            _getAllAdminStatusesResponse = await _fitCookieAIRequestExecutor.GetAllAdminStatusesAction(_fitCookieAIRequestBuilder.GetAllAdminStatusesRequestBuilder(baseFitcookieAIUri));

            if (_getAllAdminStatusesResponse.Code != null && int.Parse(_getAllAdminStatusesResponse.Code.ToString()) == 201)
            {
                model.AdminStatuses = _getAllAdminStatusesResponse.Body;
            }

            return View(model);
        }
    }
}
