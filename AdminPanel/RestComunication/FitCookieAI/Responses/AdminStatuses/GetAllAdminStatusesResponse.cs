using FitCookieAI_API.Responses;
using FitCookieAI_ApplicationService.DTOs.AdminRelated;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace AdminPanel.RestComunication.FitCookieAI.Responses.AdminStatuses
{
	public class GetAllAdminStatusesResponse : BaseResponseMessage
	{
		[JsonProperty(PropertyName = "body", NullValueHandling = NullValueHandling.Ignore)]
		public List<AdminStatusDTO> Body { get; set; }
	}
}
