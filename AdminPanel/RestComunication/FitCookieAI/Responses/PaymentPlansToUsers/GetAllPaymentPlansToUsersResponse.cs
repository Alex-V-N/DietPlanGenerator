using FitCookieAI_API.Responses;
using FitCookieAI_ApplicationService.DTOs.Others;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace AdminPanel.RestComunication.FitCookieAI.Responses.PaymentPlansToUsers
{
	public class GetAllPaymentPlansToUsersResponse : BaseResponseMessage
	{
		[JsonProperty(PropertyName = "body", NullValueHandling = NullValueHandling.Ignore)]
		public List<PaymentPlanToUserDTO> Body { get; set; }
	}
}
