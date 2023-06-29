using AdminPanel.Models.Authentication;
using AdminPanel.RestComunication.FitCookieAI;
using AdminPanel.RestComunication.FitCookieAI.Responses.Authentication;
using FitCookieAI_ApplicationService.DTOs.AdminRelated;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
	public class AuthenticationController : Controller
	{
		private readonly ILogger<AuthenticationController> _logger;

		private LoginAdminResponse _loginAdminResponse;
		private SignUpAdminResponse _signUpAdminResponse;
		private LogoutAdminResponse _logoutAdminResponse;

		private FitCookieAI_RequestBuilder _fitCookieAIRequestBuilder;
		private FitCookieAI_RequestExecutor _fitCookieAIRequestExecutor;

		string baseFitcookieAIUri;

		private IConfiguration _configuration;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthenticationController(ILogger<AuthenticationController> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
		{
			_configuration = configuration;
			_httpContextAccessor = httpContextAccessor;

			_fitCookieAIRequestBuilder = new FitCookieAI_RequestBuilder();
			_fitCookieAIRequestExecutor = new FitCookieAI_RequestExecutor(_httpContextAccessor);

			_logger = logger;
			_loginAdminResponse = new LoginAdminResponse();
			_signUpAdminResponse = new SignUpAdminResponse();
			_logoutAdminResponse = new LogoutAdminResponse();

			baseFitcookieAIUri = _configuration.GetValue<string>("FitCookieAI_API");
		}

		[HttpGet]
		public async Task<IActionResult> Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginVM model)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("Login", "Authentication");
			}

			_loginAdminResponse = await _fitCookieAIRequestExecutor.LoginAction(_fitCookieAIRequestBuilder.LoginRequestBuilder(baseFitcookieAIUri, model.Email, model.Password));

			if (_loginAdminResponse.Code != null && int.Parse(_loginAdminResponse.Code.ToString()) == 201)
			{
				this.HttpContext.Session.SetString("Email", _loginAdminResponse.Body.Email);
				this.HttpContext.Session.SetInt32("Id", _loginAdminResponse.Body.Id);
                this.HttpContext.Session.SetInt32("StatusId", _loginAdminResponse.Body.StatusId);
                this.HttpContext.Session.SetString("FirstName", _loginAdminResponse.Body.FirstName);
				this.HttpContext.Session.SetString("LastName", _loginAdminResponse.Body.LastName);

				return RedirectToAction("Index", "Home");
			}

			return RedirectToAction("Login", "Authentication");
		}

		[HttpGet]
        public async Task<IActionResult> SignUp()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpVM model)
		{
			if (!ModelState.IsValid)
			{
				return RedirectToAction("SignUp", "Authentication");
			}

			AdminDTO Admin = new AdminDTO
			{
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Password = model.Password,
				Gender = model.Gender,
				DOB = model.DOB,
				StatusId = 3
			};

			_signUpAdminResponse = await _fitCookieAIRequestExecutor.SignUpAction(Admin, _fitCookieAIRequestBuilder.SignUpRequestBuilder(baseFitcookieAIUri));

			if (_signUpAdminResponse.Code != null && int.Parse(_signUpAdminResponse.Code.ToString()) == 201)
			{
				_loginAdminResponse = await _fitCookieAIRequestExecutor.LoginAction(_fitCookieAIRequestBuilder.LoginRequestBuilder(baseFitcookieAIUri, model.Email, model.Password));

				if (_loginAdminResponse.Code != null && int.Parse(_loginAdminResponse.Code.ToString()) == 201)
				{
					this.HttpContext.Session.SetString("Email", _loginAdminResponse.Body.Email);
					this.HttpContext.Session.SetInt32("Id", _loginAdminResponse.Body.Id);
                    this.HttpContext.Session.SetInt32("StatusId", _loginAdminResponse.Body.StatusId);
                    this.HttpContext.Session.SetString("FirstName", _loginAdminResponse.Body.FirstName);
					this.HttpContext.Session.SetString("LastName", _loginAdminResponse.Body.LastName);

					return RedirectToAction("Index", "Home");
				}

				return RedirectToAction("SignUp", "Authentication");
			}

			return RedirectToAction("SignUp", "Authentication");
		}

		public async Task<IActionResult> Logout()
		{
			int AdminId = (int)this.HttpContext.Session.GetInt32("Id");
			_logoutAdminResponse = await _fitCookieAIRequestExecutor.LogoutAction(_fitCookieAIRequestBuilder.LogoutRequestBuilder(baseFitcookieAIUri, AdminId));


			if (_logoutAdminResponse != null && int.Parse(_logoutAdminResponse.Code.ToString()) == 201)
			{
				this.HttpContext.Session.Remove("Token");
				this.HttpContext.Session.Remove("RefreshToken");
				this.HttpContext.Session.Remove("Id");
                this.HttpContext.Session.Remove("StatusId");
                this.HttpContext.Session.Remove("Email");
				this.HttpContext.Session.Remove("FirstName");
				this.HttpContext.Session.Remove("LastName");

				return RedirectToAction("SignUp", "Authentication");
			}
			else
			{
				return RedirectToAction("Index", "Home", new { error = "Something went wrong, user can't Log Out!"});
			}
		}
	}
}
