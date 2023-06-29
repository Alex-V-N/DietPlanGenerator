using AdminPanel.ActionFilters;
using AdminPanel.Models.Payments;
using AdminPanel.RestComunication.FitCookieAI;
using AdminPanel.RestComunication.FitCookieAI.Responses.AdminStatuses;
using AdminPanel.RestComunication.FitCookieAI.Responses.Payments;
using AdminPanel.RestComunication.FitCookieAI.Responses.Users;
using FitCookieAI_API.Controllers.Others;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    [AdminsAuthenticationFilter]
    public class PaymentsController : Controller
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private GetAllPaymentsResponse _getAllPaymentsResponse;
        private GetAllUsersResponse _getAllUsersResponse;

        private FitCookieAI_RequestBuilder _fitCookieAIRequestBuilder;
        private FitCookieAI_RequestExecutor _fitCookieAIRequestExecutor;
        string baseFitcookieAIUri;

        public PaymentsController(IWebHostEnvironment hostEnvironment, ILogger<PaymentsController> logger,
           IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            _fitCookieAIRequestBuilder = new FitCookieAI_RequestBuilder();
            _fitCookieAIRequestExecutor = new FitCookieAI_RequestExecutor(_httpContextAccessor);

            _getAllPaymentsResponse = new GetAllPaymentsResponse();

            webHostEnvironment = hostEnvironment;
            _logger = logger;

            baseFitcookieAIUri = _configuration.GetValue<string>("FitCookieAI_API");
        }

        public async Task<IActionResult> Index()
        {
            PaymentsIndexVM model = new PaymentsIndexVM();
            _getAllPaymentsResponse = await _fitCookieAIRequestExecutor.GetAllPaymentsAction(_fitCookieAIRequestBuilder.GetAllPaymentsRequestBuilder(baseFitcookieAIUri));

            if (_getAllPaymentsResponse.Code != null && int.Parse(_getAllPaymentsResponse.Code.ToString()) == 201)
            {
                _getAllUsersResponse = await _fitCookieAIRequestExecutor.GetAllUsersAction(_fitCookieAIRequestBuilder.GetAllUsersRequestBuilder(baseFitcookieAIUri));
                if (_getAllUsersResponse.Code != null && int.Parse(_getAllUsersResponse.Code.ToString()) == 201)
                {
                    model.Users = _getAllUsersResponse.Body;
                }

                model.Payments = _getAllPaymentsResponse.Body;
            }

            return View(model);
        }
    }
}
