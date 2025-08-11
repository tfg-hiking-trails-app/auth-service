using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Domain.Entities;

namespace AuthService.Domain.Entities;

[Table("User")]
public class User : BaseEntity
{
    [Required]
    [Column("role_id")]
    public int RoleId { get; set; }

    [ForeignKey("RoleId")]
    public virtual Role? Role { get; set; }

    [Required]
    [Column("status_id")]
    public int StatusId { get; set; }

    [ForeignKey("StatusId")]
    public virtual Status? Status { get; set; }

    [Required]
    [MaxLength(50)]
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

    [Column("last_login", TypeName = "timestamp")]
    public DateTime? LastLogin { get; set; }
    
    [DefaultValue(false)]
    [Column("deleted")]
    public bool Deleted { get; set; }
    
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}