using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain.Entities;

[Table("Role")]
public class Role : BaseEntity
{
    [Required]
    [MaxLength(50)]
    [Column("role")]
    public string RoleValue { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}