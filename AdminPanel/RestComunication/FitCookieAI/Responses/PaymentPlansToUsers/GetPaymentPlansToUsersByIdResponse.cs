using FitCookieAI_API.Responses;
using FitCookieAI_ApplicationService.DTOs.Others;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace AdminPanel.RestComunication.FitCookieAI.Responses.PaymentPlansToUsers
{
	public class GetPaymentPlansToUsersByIdResponse : BaseResponseMessage
	{
		[JsonProperty(PropertyName = "body", NullValueHandling = NullValueHandling.Ignore)]
		public PaymentPlanToUserDTO Body { get; set; }
	}
}
