using FitCookieAI.RestComunication.GPT.Responses;
using Newtonsoft.Json;

namespace FitCookieAI.RestComunication.GPT
{
    public class GPT_RequestExecutor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public GPT_RequestExecutor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetGPTResponse> GetGPTResponseAction(HttpClient httpClient, string requestQuery)
        {
            GetGPTResponse _getGPTResponse = new GetGPTResponse();

            using (var response = await httpClient.GetAsync(requestQuery))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(apiResponse))
                {
                    var convert = JsonConvert.DeserializeObject<GetGPTResponse>(apiResponse);

                    if (convert != null)
                    {
                        _getGPTResponse = convert;
                    }
                }
            }

            return _getGPTResponse;
        }
    }
}
