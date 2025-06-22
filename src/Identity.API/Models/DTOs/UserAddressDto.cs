using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.DTOs
{
    public class UserAddressDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(20), Phone]
        public string Phone { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string AddressLine { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Ward { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string District { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string City { get; set; } = string.Empty;

        public bool IsDefault { get; set; } = false;
    }
}
