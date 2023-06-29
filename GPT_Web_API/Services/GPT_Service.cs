using GPT_Web_API.Requests;
using GPT_Web_API.Responses;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace GPT_Web_API.Services
{
	public class GPT_Service
	{
		private readonly IConfiguration _config;

		public GPT_Service(IConfiguration config)
		{
			_config = config;
		}

		public async Task<BaseGPTResponse> GetGPTResponse(HttpClient httpClient, BaseGPTRequest request)
		{
			BaseGPTResponse? resultResponse = new BaseGPTResponse();

			httpClient.DefaultRequestHeaders.Add("authorization", "Bearer " + _config.GetValue<string>("OpenAI_API_Key"));
			httpClient.BaseAddress = new Uri(_config.GetValue<string>("OpenAI_API_Address"));
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
					resultResponse = JsonConvert.DeserializeObject<BaseGPTResponse>(apiResponse);
				}
			}

			return resultResponse;
		}
	}
}
