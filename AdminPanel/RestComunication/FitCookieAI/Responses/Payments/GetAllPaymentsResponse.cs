using FitCookieAI_API.Responses;
using FitCookieAI_ApplicationService.DTOs.Others;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace AdminPanel.RestComunication.FitCookieAI.Responses.Payments
{
	public class GetAllPaymentsResponse : BaseResponseMessage
	{
		[JsonProperty(PropertyName = "body", NullValueHandling = NullValueHandling.Ignore)]
		public List<PaymentDTO> Body { get; set; }
	}
}
