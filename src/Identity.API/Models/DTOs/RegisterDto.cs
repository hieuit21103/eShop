using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.API.Models.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "EmailNotValid: Email is not valid.")]
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}