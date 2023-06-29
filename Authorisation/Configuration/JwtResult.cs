using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorisation.Configuration
{
	public class JwtResult
	{
		[JsonProperty(PropertyName = "token")]
		public string? Token { get; set; }

		[JsonProperty(PropertyName = "refreshToken")]
		public string? RefreshToken { get; set; }
		public bool JwtSuccess { get; set; }
		public List<string>? JwtErrors { get; set; }
	}
}
