using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain.Entities;

[Table("RefreshToken")]
public class RefreshToken : BaseEntity
{
    [Required]
    [Column("user_id")]
    public int UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User? User { get; set; }
    
    [Required]
    [Column("refresh_token")]
    public string? RefreshTokenValue { get; set; }
    
    [Required]
    [Column("active")]
    public bool Active { get; set; }
    
    [Required]
    [Column("expiration")]
    public DateTime Expiration { get; set; }
    
    [Required]
    [Column("used")]
    public bool Used { get; set; }
}