using FitCookieAI.RestComunication.FitCookieAI.Responses.PaymentRelated.Payments;
using FitCookieAI.RestComunication.FitCookieAI.Responses.UserRelated;
using FitCookieAI_ApplicationService.DTOs.Others;
using FitCookieAI_ApplicationService.DTOs.UserRelated;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FitCookieAI.RestComunication.FitCookieAI
{
    public class FitCookieAI_RequestExecutor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public FitCookieAI_RequestExecutor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //Users
        public async Task<SignUpUserResponse> SignUpAction(UserDTO request, string requestQuery)
        {
            using (var httpClient = new HttpClient())
            {
                SignUpUserResponse _signUpResponse = new SignUpUserResponse();

                httpClient.BaseAddress = new Uri(requestQuery);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = JsonConvert.SerializeObject(request);
                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (var response = await httpClient.PostAsync("", byteContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(apiResponse))
                    {
                        var convert = JsonConvert.DeserializeObject<SignUpUserResponse>(apiResponse);

                        if (convert != null)
                        {
                            _signUpResponse = convert;
                        }
                    }
                }

                return _signUpResponse;
            }
        }
        public async Task<LoginUserResponse> LoginAction(string requestQuery)
        {
            using (var httpClient = new HttpClient())
            {
                LoginUserResponse _loginUserResponse = new LoginUserResponse();

                using (var response = await httpClient.GetAsync(requestQuery))
                {
                    if (response.Headers.FirstOrDefault(x => x.Key == "token").Value != null ||
                        response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value != null)
                    {
                        _session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
                        _session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());

                        string apiResponse = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(apiResponse))
                        {
                            var convert = JsonConvert.DeserializeObject<LoginUserResponse>(apiResponse);

                            if (convert != null)
                            {
                                _loginUserResponse = convert;
                            }
                        }
                    }
                }

                return _loginUserResponse;
            }

        }
        public async Task<LogoutUserResponse> LogoutAction(string requestQuery)
        {
            using (var httpClient = new HttpClient())
            {
                LogoutUserResponse _logoutResponse = new LogoutUserResponse();

                using (var response = await httpClient.GetAsync(requestQuery))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(apiResponse))
                    {
                        var convert = JsonConvert.DeserializeObject<LogoutUserResponse>(apiResponse);

                        if (convert != null)
                        {
                            _logoutResponse = convert;
                        }
                    }
                }

                return _logoutResponse;
            }
        }
        public async Task<UpdateUsersResponse> UpdateUsersAction(UserDTO request, string requestQuery)
        {
            using (var httpClient = new HttpClient())
            {
                UpdateUsersResponse _updateUserResponse = new UpdateUsersResponse();

                httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
                httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

                httpClient.BaseAddress = new Uri(requestQuery);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = JsonConvert.SerializeObject(request);
                var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using (var response = await httpClient.PostAsync("", byteContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(apiResponse))
                    {
                        var convert = JsonConvert.DeserializeObject<UpdateUsersResponse>(apiResponse);

                        if (convert != null)
                        {
                            _updateUserResponse = convert;

                            _session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
                            _session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
                        }
                    }
                }
                return _updateUserResponse;
            }
        }

		//Payments
		public async Task<ChargePaymentResponse> ChargePaymentsAction(PaymentDTO request, string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				ChargePaymentResponse _savePaymentResponse = new ChargePaymentResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				httpClient.BaseAddress = new Uri(requestQuery);
				httpClient.DefaultRequestHeaders.Accept.Clear();
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var content = JsonConvert.SerializeObject(request);
				var buffer = System.Text.Encoding.UTF8.GetBytes(content);
				var byteContent = new ByteArrayContent(buffer);
				byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				using (var response = await httpClient.PostAsync("", byteContent))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<ChargePaymentResponse>(apiResponse);

						if (convert != null)
						{
							_savePaymentResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}
				return _savePaymentResponse;
			}
		}
	}
}
