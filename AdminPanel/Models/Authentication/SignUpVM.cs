using System.ComponentModel.DataAnnotations;

namespace AdminPanel.Models.Authentication
{
    public class SignUpVM
    {
        [Required(ErrorMessage = "This field is required!")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        public string? Gender { get; set; }
    }
}
