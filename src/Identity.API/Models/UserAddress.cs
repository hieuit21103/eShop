using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.API.Models
{
    [Table("UserAddresses")]
    public class UserAddress
    {
        [Key]
        public Guid Id { get; set; }
        public string? UserId { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(20)]
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

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }

}