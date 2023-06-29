using FitCookieAI_API.Responses;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace FitCookieAI.RestComunication.FitCookieAI.Responses.PaymentRelated.Payments
{
    public class ChargePaymentResponse : BaseResponseMessage
    {
        [JsonProperty(PropertyName = "body", NullValueHandling = NullValueHandling.Ignore)]
        public object Body { get; set; }
    }
}
