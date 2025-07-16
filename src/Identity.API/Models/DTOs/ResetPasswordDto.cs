namespace Identity.API.Models.DTOs
{
    public class ResetPasswordDto
    {
        [Required, StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required, StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}