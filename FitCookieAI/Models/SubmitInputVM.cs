using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace FitCookieAI.Models
{
	public class SubmitInputVM
	{
        public bool HasAccount { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "This field is required!")]
		public string? Gender { get; set; }

		[Required(ErrorMessage = "This field is required!")]
		public DateTime DOB { get; set; }

		[Required(ErrorMessage = "This field is required!")]
		public double Height { get; set; }

        [Required(ErrorMessage = "This field is required!")]
		public double Weight { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        public double TargetWeight { get; set; }

        [Required(ErrorMessage = "This field is required!")]
		public string ActivityLevel { get; set; }
		public string DietaryRestrictions { get; set; }
		public string FoodPreferences { get; set; }

		[Required(ErrorMessage = "This field is required!")]
		public string HealthGoal { get; set; }
        public double BMI { get; set; }
		public string? Ocupation { get; set; }
    }
}
