using AdminPanel.ActionFilters;
using AdminPanel.Models.PaymentPlans;
using AdminPanel.RestComunication.FitCookieAI;
using AdminPanel.RestComunication.FitCookieAI.Responses.PaymentPlanFeatures;
using AdminPanel.RestComunication.FitCookieAI.Responses.PaymentPlans;
using AdminPanel.RestComunication.FitCookieAI.Responses.Payments;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    [AdminsAuthenticationFilter]
    public class PaymentPlansController : Controller
    {
        private readonly ILogger<PaymentPlansController> _logger;
        private readonly IWebHostEnvironment webHostEnvironment;
        private IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private GetAllPaymentPlansResponse _getAllPaymentPlansResponse;
        private GetAllPaymentPlanFeaturesResponse _getAllPaymentPlanFeaturesResponse;

        private FitCookieAI_RequestBuilder _fitCookieAIRequestBuilder;
        private FitCookieAI_RequestExecutor _fitCookieAIRequestExecutor;
        string baseFitcookieAIUri;

        public PaymentPlansController(IWebHostEnvironment hostEnvironment, ILogger<PaymentPlansController> logger,
           IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            _fitCookieAIRequestBuilder = new FitCookieAI_RequestBuilder();
            _fitCookieAIRequestExecutor = new FitCookieAI_RequestExecutor(_httpContextAccessor);

            _getAllPaymentPlanFeaturesResponse = new GetAllPaymentPlanFeaturesResponse();
            _getAllPaymentPlansResponse = new GetAllPaymentPlansResponse();

            webHostEnvironment = hostEnvironment;
            _logger = logger;

            baseFitcookieAIUri = _configuration.GetValue<string>("FitCookieAI_API");
        }

        public async Task<IActionResult> Index()
        {
            PaymentPlansIndexVM model = new PaymentPlansIndexVM();
            _getAllPaymentPlansResponse = await _fitCookieAIRequestExecutor.GetAllPaymentPlansAction(
                _fitCookieAIRequestBuilder.GetAllPaymentPlansRequestBuilder(baseFitcookieAIUri));

            if (_getAllPaymentPlansResponse.Code != null && int.Parse(_getAllPaymentPlansResponse.Code.ToString()) == 201)
            {
                _getAllPaymentPlanFeaturesResponse = await _fitCookieAIRequestExecutor.GetAllPaymentPlanFeaturesAction(
                    _fitCookieAIRequestBuilder.GetAllPaymentPlanFeaturesRequestBuilder(baseFitcookieAIUri));

                if (_getAllPaymentPlanFeaturesResponse.Code != null && int.Parse(_getAllPaymentPlanFeaturesResponse.Code.ToString()) == 201)
                {
                    model.PaymentPlanFeatures = _getAllPaymentPlanFeaturesResponse.Body;
                }

                model.PaymentPlans = _getAllPaymentPlansResponse.Body;
            }

            return View(model);
        }
    }
}
