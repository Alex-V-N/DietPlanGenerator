using System.ComponentModel.DataAnnotations;

namespace FitCookieAI.Models
{
	public class SignUpVM
	{
		[Required]
		public string? Email { get; set; }

		[Required]
		public string? Password { get; set; }

		[Required]
		public string? ConfirmPassword { get; set; }

		[Required]
		public string? FirstName { get; set; }

		[Required]
		public string? LastName { get; set; }

		[Required]
		public string? Gender { get; set; }

		[Required]
		public DateTime DOB { get; set; }
	}
}
