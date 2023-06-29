using System.ComponentModel.DataAnnotations;

namespace FitCookieAI.Models
{
	public class LoginVM
	{
		[Required]
		public string? Email { get; set; }

		[Required]
		public string? Password { get; set; }
	}
}
