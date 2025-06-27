using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain
{
    [Table("Status")]
    internal class Status : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        [Column("status")]
        public string StatusValue { get; set; } = string.Empty;

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
