using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.DTOs
{
    public class UserProfileDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string AvatarUrl { get; set; } = string.Empty;
    }
}