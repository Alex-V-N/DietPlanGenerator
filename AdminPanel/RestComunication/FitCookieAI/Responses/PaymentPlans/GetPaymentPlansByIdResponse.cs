using FitCookieAI_API.Responses;
using FitCookieAI_ApplicationService.DTOs.Others;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace AdminPanel.RestComunication.FitCookieAI.Responses.PaymentPlans
{
	public class GetPaymentPlansByIdResponse : BaseResponseMessage
	{
		[JsonProperty(PropertyName = "body", NullValueHandling = NullValueHandling.Ignore)]
		public PaymentPlanDTO Body { get; set; }
	}
}
