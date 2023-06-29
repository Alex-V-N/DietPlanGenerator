using FitCookieAI_API.Responses;
using FitCookieAI_ApplicationService.DTOs.Others;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace AdminPanel.RestComunication.FitCookieAI.Responses.PaymentPlanFeatures
{
	public class GetPaymentPlanFeaturesByIdResponse : BaseResponseMessage
	{
		[JsonProperty(PropertyName = "body", NullValueHandling = NullValueHandling.Ignore)]
		public PaymentPlanFeatureDTO Body { get; set; }
	}
}
