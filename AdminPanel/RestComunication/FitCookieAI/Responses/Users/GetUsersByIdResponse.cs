using FitCookieAI_API.Responses;
using FitCookieAI_ApplicationService.DTOs.UserRelated;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace AdminPanel.RestComunication.FitCookieAI.Responses.Users
{
	public class GetUsersByIdResponse : BaseResponseMessage
	{
		[JsonProperty(PropertyName = "body", NullValueHandling = NullValueHandling.Ignore)]
		public UserDTO Body { get; set; }
	}
}
