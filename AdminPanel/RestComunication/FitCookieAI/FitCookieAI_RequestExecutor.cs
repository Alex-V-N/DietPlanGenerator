using AdminPanel.RestComunication.FitCookieAI.Responses.Admins;
using AdminPanel.RestComunication.FitCookieAI.Responses.AdminStatuses;
using AdminPanel.RestComunication.FitCookieAI.Responses.Authentication;
using AdminPanel.RestComunication.FitCookieAI.Responses.PaymentPlanFeatures;
using AdminPanel.RestComunication.FitCookieAI.Responses.PaymentPlans;
using AdminPanel.RestComunication.FitCookieAI.Responses.PaymentPlansToUsers;
using AdminPanel.RestComunication.FitCookieAI.Responses.Payments;
using AdminPanel.RestComunication.FitCookieAI.Responses.Users;
using FitCookieAI_ApplicationService.DTOs.AdminRelated;
using FitCookieAI_ApplicationService.DTOs.Others;
using FitCookieAI_ApplicationService.DTOs.UserRelated;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AdminPanel.RestComunication.FitCookieAI
{
    public class FitCookieAI_RequestExecutor
    {
		private readonly IHttpContextAccessor _httpContextAccessor;
		private ISession _session => _httpContextAccessor.HttpContext.Session;

		public FitCookieAI_RequestExecutor(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		//Admins
		public async Task<SignUpAdminResponse> SignUpAction(AdminDTO request, string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				SignUpAdminResponse _signUpResponse = new SignUpAdminResponse();

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
						var convert = JsonConvert.DeserializeObject<SignUpAdminResponse>(apiResponse);

						if (convert != null)
						{
							_signUpResponse = convert;
						}
					}
				}

				return _signUpResponse;
			}
		}
		public async Task<LoginAdminResponse> LoginAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				LoginAdminResponse _loginAdminResponse = new LoginAdminResponse();

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
							var convert = JsonConvert.DeserializeObject<LoginAdminResponse>(apiResponse);

							if (convert != null)
							{
								_loginAdminResponse = convert;
							}
						}
					}
				}

				return _loginAdminResponse;
			}

		}
		public async Task<LogoutAdminResponse> LogoutAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				LogoutAdminResponse _logoutResponse = new LogoutAdminResponse();

				using (var response = await httpClient.GetAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<LogoutAdminResponse>(apiResponse);

						if (convert != null)
						{
							_logoutResponse = convert;
						}
					}
				}

				return _logoutResponse;
			}
		}
		public async Task<UpdateAdminsResponse> UpdateAdminsAction(AdminDTO request, string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				UpdateAdminsResponse _updateAdminResponse = new UpdateAdminsResponse();

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
						var convert = JsonConvert.DeserializeObject<UpdateAdminsResponse>(apiResponse);

						if (convert != null)
						{
							_updateAdminResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}
				return _updateAdminResponse;
			}
		}
		public async Task<GetAllAdminsResponse> GetAllAdminsAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				GetAllAdminsResponse _getAllAdminsRespponse = new GetAllAdminsResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.GetAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<GetAllAdminsResponse>(apiResponse);

						if (convert != null)
						{
							_getAllAdminsRespponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _getAllAdminsRespponse;
			}
		}
		public async Task<DeleteAdminsResponse> DeleteAdminsAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				DeleteAdminsResponse _deleteAdminsResponse = new DeleteAdminsResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.DeleteAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<DeleteAdminsResponse>(apiResponse);

						if (convert != null)
						{
							_deleteAdminsResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _deleteAdminsResponse;
			}
		}
		public async Task<GetAdminsByIdResponse> GetAdminsByIdAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				GetAdminsByIdResponse _getAdminsByIdResponse = new GetAdminsByIdResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.GetAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<GetAdminsByIdResponse>(apiResponse);

						if (convert != null)
						{
							_getAdminsByIdResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _getAdminsByIdResponse;
			}
		}

		//Payments
		public async Task<GetAllPaymentsResponse> GetAllPaymentsAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				GetAllPaymentsResponse _getAllPaymentsRespponse = new GetAllPaymentsResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.GetAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<GetAllPaymentsResponse>(apiResponse);

						if (convert != null)
						{
							_getAllPaymentsRespponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _getAllPaymentsRespponse;
			}
		}
		public async Task<DeletePaymentsResponse> DeletePaymentsAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				DeletePaymentsResponse _deletePaymentsResponse = new DeletePaymentsResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.DeleteAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<DeletePaymentsResponse>(apiResponse);

						if (convert != null)
						{
							_deletePaymentsResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _deletePaymentsResponse;
			}
		}
		public async Task<SavePaymentsResponse> SavePaymentsAction(PaymentDTO request, string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				SavePaymentsResponse _savePaymentsResponse = new SavePaymentsResponse();

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
						var convert = JsonConvert.DeserializeObject<SavePaymentsResponse>(apiResponse);

						if (convert != null)
						{
							_savePaymentsResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _savePaymentsResponse;
			}
		}
		public async Task<GetPaymentsByIdResponse> GetPaymentsByIdAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				GetPaymentsByIdResponse _getPaymentsByIdResponse = new GetPaymentsByIdResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.GetAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<GetPaymentsByIdResponse>(apiResponse);

						if (convert != null)
						{
							_getPaymentsByIdResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _getPaymentsByIdResponse;
			}
		}

		//PaymentPlans
		public async Task<GetAllPaymentPlansResponse> GetAllPaymentPlansAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				GetAllPaymentPlansResponse _getAllPaymentPlansRespponse = new GetAllPaymentPlansResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.GetAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<GetAllPaymentPlansResponse>(apiResponse);

						if (convert != null)
						{
							_getAllPaymentPlansRespponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _getAllPaymentPlansRespponse;
			}
		}
		public async Task<DeletePaymentPlansResponse> DeletePaymentPlansAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				DeletePaymentPlansResponse _deletePaymentPlansResponse = new DeletePaymentPlansResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.DeleteAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<DeletePaymentPlansResponse>(apiResponse);

						if (convert != null)
						{
							_deletePaymentPlansResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _deletePaymentPlansResponse;
			}
		}
		public async Task<SavePaymentPlansResponse> SavePaymentPlansAction(PaymentPlanDTO request, string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				SavePaymentPlansResponse _savePaymentPlansResponse = new SavePaymentPlansResponse();

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
						var convert = JsonConvert.DeserializeObject<SavePaymentPlansResponse>(apiResponse);

						if (convert != null)
						{
							_savePaymentPlansResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _savePaymentPlansResponse;
			}
		}
		public async Task<GetPaymentPlansByIdResponse> GetPaymentPlansByIdAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				GetPaymentPlansByIdResponse _getPaymentPlansByIdResponse = new GetPaymentPlansByIdResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.GetAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<GetPaymentPlansByIdResponse>(apiResponse);

						if (convert != null)
						{
							_getPaymentPlansByIdResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _getPaymentPlansByIdResponse;
			}
		}

		//PaymentPlanFeatures
		public async Task<GetAllPaymentPlanFeaturesResponse> GetAllPaymentPlanFeaturesAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				GetAllPaymentPlanFeaturesResponse _getAllPaymentPlanFeaturesRespponse = new GetAllPaymentPlanFeaturesResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.GetAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<GetAllPaymentPlanFeaturesResponse>(apiResponse);

						if (convert != null)
						{
							_getAllPaymentPlanFeaturesRespponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _getAllPaymentPlanFeaturesRespponse;
			}
		}
		public async Task<DeletePaymentPlanFeaturesResponse> DeletePaymentPlanFeaturesAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				DeletePaymentPlanFeaturesResponse _deletePaymentPlanFeaturesResponse = new DeletePaymentPlanFeaturesResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.DeleteAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<DeletePaymentPlanFeaturesResponse>(apiResponse);

						if (convert != null)
						{
							_deletePaymentPlanFeaturesResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _deletePaymentPlanFeaturesResponse;
			}
		}
		public async Task<SavePaymentPlanFeaturesResponse> SavePaymentPlanFeaturesAction(PaymentPlanFeatureDTO request, string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				SavePaymentPlanFeaturesResponse _savePaymentPlanFeaturesResponse = new SavePaymentPlanFeaturesResponse();

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
						var convert = JsonConvert.DeserializeObject<SavePaymentPlanFeaturesResponse>(apiResponse);

						if (convert != null)
						{
							_savePaymentPlanFeaturesResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _savePaymentPlanFeaturesResponse;
			}
		}
		public async Task<GetPaymentPlanFeaturesByIdResponse> GetPaymentPlanFeaturesByIdAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				GetPaymentPlanFeaturesByIdResponse _getPaymentPlanFeaturesByIdResponse = new GetPaymentPlanFeaturesByIdResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.GetAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<GetPaymentPlanFeaturesByIdResponse>(apiResponse);

						if (convert != null)
						{
							_getPaymentPlanFeaturesByIdResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _getPaymentPlanFeaturesByIdResponse;
			}
		}

		//Users
		public async Task<GetAllUsersResponse> GetAllUsersAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				GetAllUsersResponse _getAllUsersRespponse = new GetAllUsersResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.GetAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<GetAllUsersResponse>(apiResponse);

						if (convert != null)
						{
							_getAllUsersRespponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _getAllUsersRespponse;
			}
		}
		public async Task<DeleteUsersResponse> DeleteUsersAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				DeleteUsersResponse _deleteUsersResponse = new DeleteUsersResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.DeleteAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<DeleteUsersResponse>(apiResponse);

						if (convert != null)
						{
							_deleteUsersResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _deleteUsersResponse;
			}
		}
		public async Task<UpdateUsersResponse> UpdateUsersAction(UserDTO request, string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				UpdateUsersResponse _saveUsersResponse = new UpdateUsersResponse();

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
							_saveUsersResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _saveUsersResponse;
			}
		}
		public async Task<GetUsersByIdResponse> GetUsersByIdAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				GetUsersByIdResponse _getUsersByIdResponse = new GetUsersByIdResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.GetAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<GetUsersByIdResponse>(apiResponse);

						if (convert != null)
						{
							_getUsersByIdResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _getUsersByIdResponse;
			}
		}

		//AdminStatuses
		public async Task<GetAllAdminStatusesResponse> GetAllAdminStatusesAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				GetAllAdminStatusesResponse _getAllAdminStatusesRespponse = new GetAllAdminStatusesResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.GetAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<GetAllAdminStatusesResponse>(apiResponse);

						if (convert != null)
						{
							_getAllAdminStatusesRespponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _getAllAdminStatusesRespponse;
			}
		}
		public async Task<DeleteAdminStatusesResponse> DeleteAdminStatusesAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				DeleteAdminStatusesResponse _deleteAdminStatusesResponse = new DeleteAdminStatusesResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.DeleteAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<DeleteAdminStatusesResponse>(apiResponse);

						if (convert != null)
						{
							_deleteAdminStatusesResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _deleteAdminStatusesResponse;
			}
		}
		public async Task<SaveAdminStatusesResponse> SaveAdminStatusesAction(AdminStatusDTO request, string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				SaveAdminStatusesResponse _saveAdminStatusesResponse = new SaveAdminStatusesResponse();

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
						var convert = JsonConvert.DeserializeObject<SaveAdminStatusesResponse>(apiResponse);

						if (convert != null)
						{
							_saveAdminStatusesResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _saveAdminStatusesResponse;
			}
		}
		public async Task<GetAdminStatusesByIdResponse> GetAdminStatusesByIdAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				GetAdminStatusesByIdResponse _getAdminStatusesByIdResponse = new GetAdminStatusesByIdResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.GetAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<GetAdminStatusesByIdResponse>(apiResponse);

						if (convert != null)
						{
							_getAdminStatusesByIdResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _getAdminStatusesByIdResponse;
			}
		}

		//PaymentPlansToUsers
		public async Task<GetAllPaymentPlansToUsersResponse> GetAllPaymentPlansToUsersAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				GetAllPaymentPlansToUsersResponse _getAllPaymentPlansToUsersRespponse = new GetAllPaymentPlansToUsersResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.GetAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<GetAllPaymentPlansToUsersResponse>(apiResponse);

						if (convert != null)
						{
							_getAllPaymentPlansToUsersRespponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _getAllPaymentPlansToUsersRespponse;
			}
		}
		public async Task<DeletePaymentPlansToUsersResponse> DeletePaymentPlansToUsersAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				DeletePaymentPlansToUsersResponse _deletePaymentPlansToUsersResponse = new DeletePaymentPlansToUsersResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.DeleteAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<DeletePaymentPlansToUsersResponse>(apiResponse);

						if (convert != null)
						{
							_deletePaymentPlansToUsersResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _deletePaymentPlansToUsersResponse;
			}
		}
		public async Task<SavePaymentPlansToUsersResponse> SavePaymentPlansToUsersAction(PaymentPlanToUserDTO request, string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				SavePaymentPlansToUsersResponse _savePaymentPlansToUsersResponse = new SavePaymentPlansToUsersResponse();

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
						var convert = JsonConvert.DeserializeObject<SavePaymentPlansToUsersResponse>(apiResponse);

						if (convert != null)
						{
							_savePaymentPlansToUsersResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _savePaymentPlansToUsersResponse;
			}
		}
		public async Task<GetPaymentPlansToUsersByIdResponse> GetPaymentPlansToUsersByIdAction(string requestQuery)
		{
			using (var httpClient = new HttpClient())
			{
				GetPaymentPlansToUsersByIdResponse _getPaymentPlansToUsersByIdResponse = new GetPaymentPlansToUsersByIdResponse();

				httpClient.DefaultRequestHeaders.Add("token", _session.GetString("Token"));
				httpClient.DefaultRequestHeaders.Add("refreshToken", _session.GetString("RefreshToken"));

				using (var response = await httpClient.GetAsync(requestQuery))
				{
					string apiResponse = await response.Content.ReadAsStringAsync();

					if (!string.IsNullOrEmpty(apiResponse))
					{
						var convert = JsonConvert.DeserializeObject<GetPaymentPlansToUsersByIdResponse>(apiResponse);

						if (convert != null)
						{
							_getPaymentPlansToUsersByIdResponse = convert;

							_session.SetString("Token", response.Headers.FirstOrDefault(x => x.Key == "token").Value.FirstOrDefault());
							_session.SetString("RefreshToken", response.Headers.FirstOrDefault(x => x.Key == "refreshToken").Value.FirstOrDefault());
						}
					}
				}

				return _getPaymentPlansToUsersByIdResponse;
			}
		}
	}
}
