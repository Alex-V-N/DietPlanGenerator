using FitCookieAI_API.Responses;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace AdminPanel.RestComunication.FitCookieAI.Responses.Admins
{
	public class DeleteAdminsResponse : BaseResponseMessage
	{
		[JsonProperty(PropertyName = "body", NullValueHandling = NullValueHandling.Ignore)]
		public string Body { get; set; }
	}
}
