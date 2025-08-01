using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Domain.Entities;

namespace AuthService.Domain.Entities;

[Table("Status")]
public class Status : BaseEntity
{
    [Required]
    [MaxLength(50)]
    [Column("status")]
    public string StatusValue { get; set; } = string.Empty;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}