using FitCookieAI_ApplicationService.DTOs.AdminRelated;
using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models.Admins
{
	public class ProfileVM
	{
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public DateTime? DOB { get; set; }

        [Required]
        public string? Gender { get; set; }
        public int StatusId { get; set; }
        public string? NewPassword { get; set; }
		public string? Error { get; set; }
		public string? Message { get; set; }
        public string? ProfilePhotoName { get; set; }
        public IFormFile FileName { get; set; }
    }
}
