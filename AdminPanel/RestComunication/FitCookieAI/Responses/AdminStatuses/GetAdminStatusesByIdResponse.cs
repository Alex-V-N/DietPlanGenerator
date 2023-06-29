using FitCookieAI_API.Responses;
using FitCookieAI_ApplicationService.DTOs.AdminRelated;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace AdminPanel.RestComunication.FitCookieAI.Responses.AdminStatuses
{
	public class GetAdminStatusesByIdResponse : BaseResponseMessage
	{
		[JsonProperty(PropertyName = "body", NullValueHandling = NullValueHandling.Ignore)]
		public AdminStatusDTO Body { get; set; }
	}
}
