using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain
{
    [Table("User")]
    internal class User : BaseEntity
    {
        [Required]
        [Column("role_id")]
        public int RoleId { get; set; }

        [Required]
        [Column("status_id")]
        public int StatusId { get; set; }

        [Required]
        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(50)]
        [Column("last_name")]
        public string LastName { get; set; } = string.Empty;

        [Column("date_of_birth", TypeName = "date")]
        public DateTime? DateOfBirth { get; set; }

        [Column("last_login", TypeName = "timestamp")]
        public DateTime? LastLogin { get; set; }

        [MaxLength(255)]
        [Column("profile_picture_url")]
        public string ProfilePictureUrl { get; set; } = string.Empty;

        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }

        [ForeignKey("StatusId")]
        public virtual Status? Status { get; set; }
    }
}
