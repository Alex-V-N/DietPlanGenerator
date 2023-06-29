using DocuSign.eSign.Model;
using FitCookieAI.Models;
using FitCookieAI.RestComunication.FitCookieAI;
using FitCookieAI.RestComunication.FitCookieAI.Responses.PaymentRelated.Payments;
using FitCookieAI.RestComunication.FitCookieAI.Responses.UserRelated;
using FitCookieAI.RestComunication.GPT;
using FitCookieAI.RestComunication.GPT.Responses;
using FitCookieAI_ApplicationService.DTOs.Others;
using FitCookieAI_ApplicationService.DTOs.UserRelated;
using FitCookieAI_Data.Entities.UserRelated;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Mail;

namespace FitCookieAI.Controllers
{
    public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		private GetGPTResponse _GPTResponse;
		private LoginUserResponse _loginUserResponse;
		private SignUpUserResponse _signUpUserResponse;
		private LogoutUserResponse _logoutUserResponse;

		private GPT_RequestBuilder _GPTRequestBuilder;
		private GPT_RequestExecutor _GPTRequestExecutor;
		private FitCookieAI_RequestBuilder _fitCookieAIRequestBuilder;
		private FitCookieAI_RequestExecutor _fitCookieAIRequestExecutor;
		private ChargePaymentResponse _chargePaymentResponse;

		string baseGPTUri;
		string baseFitcookieAIUri;
		string stripePublicKey;

		private IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
		{
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;

			_GPTRequestBuilder= new GPT_RequestBuilder();
			_GPTRequestExecutor = new GPT_RequestExecutor(_httpContextAccessor);
			_fitCookieAIRequestBuilder = new FitCookieAI_RequestBuilder();
			_fitCookieAIRequestExecutor = new FitCookieAI_RequestExecutor(_httpContextAccessor);

			_logger = logger;
			_GPTResponse = new GetGPTResponse();
			_loginUserResponse = new LoginUserResponse();
			_signUpUserResponse	= new SignUpUserResponse();
			_logoutUserResponse= new LogoutUserResponse();
			_chargePaymentResponse = new ChargePaymentResponse();
			
			baseGPTUri = _configuration.GetValue<string>("GPT_API");
			baseFitcookieAIUri = _configuration.GetValue<string>("FitCookieAI_API");
			stripePublicKey = _configuration.GetSection("Stripe")["PublicKey"];
		}

		public IActionResult Index()
		{
			this.HttpContext.Session.SetString("PublicKey", stripePublicKey);

			return View();
		}

		public async Task<JsonResult> Login(LoginVM model)
		{
			if (!ModelState.IsValid)
			{
				return Json(new {code = 400, message = "Login input is invalid (missing data)!" });
			}

			_loginUserResponse = await _fitCookieAIRequestExecutor.LoginAction(_fitCookieAIRequestBuilder.LoginRequestBuilder(baseFitcookieAIUri, model.Email, model.Password));

			if (_loginUserResponse.Code != null && int.Parse(_loginUserResponse.Code.ToString()) == 201)
			{
				this.HttpContext.Session.SetString("Email", _loginUserResponse.Body.Email);
				this.HttpContext.Session.SetInt32("Id", _loginUserResponse.Body.Id);
				this.HttpContext.Session.SetString("FirstName", _loginUserResponse.Body.FirstName);
				this.HttpContext.Session.SetString("LastName", _loginUserResponse.Body.LastName);

				return Json(new { code = 201, message = "SLogin was successful!", names = _loginUserResponse.Body.FirstName + " " + _loginUserResponse.Body.LastName });
			}

			return Json(new { code = 200, message = "A user with this set of credentials doesn't exist, please check your" });
		}

		public async Task<JsonResult> SignUp(SignUpVM model)
		{
			if (!ModelState.IsValid)
			{
				return Json(new { code = 400, message = "SignUp input is invalid (missing data)!" });
			}
			if (model.Password != model.ConfirmPassword)
			{
				return Json(new { code = 400, message = "SignUp input is invalid (password != confirm pasword)!" });
			}

			UserDTO user = new UserDTO
			{
				Email= model.Email,
				FirstName= model.FirstName,
				LastName= model.LastName,
				Password= model.Password,
				Gender= model.Gender,
				DOB= model.DOB
			};

			_signUpUserResponse = await _fitCookieAIRequestExecutor.SignUpAction(user, _fitCookieAIRequestBuilder.SignUpRequestBuilder(baseFitcookieAIUri));

			if (_signUpUserResponse.Code != null && int.Parse(_signUpUserResponse.Code.ToString()) == 201)
			{
				_loginUserResponse = await _fitCookieAIRequestExecutor.LoginAction(_fitCookieAIRequestBuilder.LoginRequestBuilder(baseFitcookieAIUri, model.Email, model.Password));

				if (_loginUserResponse.Code != null && int.Parse(_loginUserResponse.Code.ToString()) == 201)
				{
					this.HttpContext.Session.SetString("Email", _loginUserResponse.Body.Email);
					this.HttpContext.Session.SetInt32("Id", _loginUserResponse.Body.Id);
					this.HttpContext.Session.SetString("FirstName", _loginUserResponse.Body.FirstName);
					this.HttpContext.Session.SetString("LastName", _loginUserResponse.Body.LastName);

					return Json(new { code = 201, message = "SignUp and Login were successful!", names = _loginUserResponse.Body.FirstName + " " + _loginUserResponse.Body.LastName });
				}

				return Json(new { code = 200, message = "SignUp was successful but Login failed!" });
			}

			return Json(new { code = 200, message = "SignUp failed!" });
		}

		public async Task<JsonResult> SubmitInput(SubmitInputVM model)
		{
			if (model.Height != 0 && model.Weight != 0 && model.TargetWeight != 0 && !string.IsNullOrEmpty(model.Ocupation) &&
				model.BMI != 0 && !string.IsNullOrEmpty(model.HealthGoal) && !string.IsNullOrEmpty(model.ActivityLevel))
			{

			}

			string dietaryRestrictions = "";
			string foodPreferences = "";
			if (!string.IsNullOrEmpty(model.DietaryRestrictions))
			{
				dietaryRestrictions = $"My dietary restrictions/preferences are: {model.DietaryRestrictions}, ";
			}
			if (!string.IsNullOrEmpty(model.FoodPreferences))
			{
				foodPreferences = $"My food preferences are: {model.FoodPreferences}, ";
			}

			TimeSpan age = DateTime.Now.Subtract(model.DOB);
			int years = (int)(age.TotalDays / 365.25);

			string request = $"As a profesional dietitian recoment me a diet plan, I am a {model.Gender}, {years} years old," + $" my name is {model.FirstName} + {model.LastName}" +
				$" I weigh {model.Weight} kilograms and my target weight is {model.TargetWeight}, my height is {model.Height} meters, my BMI is {model.BMI}, my activity level is {model.ActivityLevel}" + 
				dietaryRestrictions + foodPreferences + $", my healt goal is to {model.HealthGoal}, and my oocupations is {model.Ocupation}. Also recomend me a list of suplements which would help me based on my needs and goals " +
				$"and briefly explain the benefit of each of them." +$"The plan has to contain 3 to 5 meals, depending on the activity level, and at the end of each meal you have to display the approximate calories, protein, carbs and fats.";

			using (var httpClient = new HttpClient())
			{
				var requestUrl = "https://localhost:7062/api/GPT/Get?input=" + request;
				string requestQuery = _GPTRequestBuilder.PostGPTINputRequestBuilder(baseGPTUri, request);
				_GPTResponse = await _GPTRequestExecutor.GetGPTResponseAction(httpClient, requestQuery);
				//obj = await httpClient.GetAsync(requestUrl);
			}

			return Json(_GPTResponse.choices[0].text);
		}

		[HttpPost]
		public async Task<JsonResult> StripePayment(PaymentDTO paymentData)
		{
			if (paymentData != null)
			{
				paymentData.Currency = "usd";
				paymentData.UserId = (int)this.HttpContext.Session.GetInt32("Id");
				 
				_chargePaymentResponse = await _fitCookieAIRequestExecutor
					.ChargePaymentsAction(paymentData, _fitCookieAIRequestBuilder.ChargePaymentsRequestBuilder(baseFitcookieAIUri));

				if (_chargePaymentResponse.Code != null && int.Parse(_chargePaymentResponse.Code.ToString()) == 201)
				{
					return Json(new { status = 201, response = _chargePaymentResponse.Body });
				}
				else
				{
					return Json(new { status = int.Parse(_chargePaymentResponse.Code.ToString()), response = _chargePaymentResponse.Error });
				}
			}
			else
			{
				return Json(new {status = 400, response = new {message = "Input data was invalid!" } });
			}
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[HttpPost]
        public async Task<JsonResult> IsInKioskMode(bool IsInKioskMode)
        {
			string mode = "";

			if (IsInKioskMode == true)
			{
				HttpContext.Session.SetString("Kiosk", "true");
				mode = "Kiosk mode";
				return Json(mode);
			}
			else 
			{
				HttpContext.Session.SetString("Kiosk", "false");
				mode = "Normal mode";
				return Json(mode);
			}
        }

		[HttpPost]
		public async Task<JsonResult> Logout()
		{
			int userId = (int)this.HttpContext.Session.GetInt32("Id");
			_logoutUserResponse = await _fitCookieAIRequestExecutor.LogoutAction(_fitCookieAIRequestBuilder.LogoutRequestBuilder(baseFitcookieAIUri, userId));


			if (_logoutUserResponse != null && int.Parse(_logoutUserResponse.Code.ToString()) == 201)
			{
				this.HttpContext.Session.Remove("Token");
				this.HttpContext.Session.Remove("RefreshToken");
				this.HttpContext.Session.Remove("Id");
				this.HttpContext.Session.Remove("Email");
				this.HttpContext.Session.Remove("FirstName");
				this.HttpContext.Session.Remove("LastName");

				return Json(new { code = 201, message = "Logout failed!" });
			}
			else
			{
				return Json(new { code = 200, message = "Logout failed!" });
			}
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}