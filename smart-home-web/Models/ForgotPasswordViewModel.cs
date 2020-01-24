using System.ComponentModel.DataAnnotations;

namespace smart_home_web.Models
{
	public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
