using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace FitCookieAI_API.Responses
{
    public class BaseResponseMessage
    {
        [JsonProperty(PropertyName = "code")]
        public object? Code { get; set; }

        [JsonProperty(PropertyName = "body", NullValueHandling = NullValueHandling.Ignore)]
        public object? Body { get; set; }

        [JsonProperty(PropertyName = "error", NullValueHandling = NullValueHandling.Ignore)]
        public object? Error { get; set; }
        public bool JwtSuccess { get; set; }
        public List<string>? JwtErrors { get; set; }
    }
}
