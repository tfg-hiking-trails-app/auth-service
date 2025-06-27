using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Domain
{
    internal class BaseEntity
    {
        [Key]
        [Column("dd")]
        public int Id { get; set; }

        [Required]
        [Column("code")]
        public Guid Code { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("created_at", TypeName = "timestamp")]
        public DateTime CreatedAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("updated_at", TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }
    }
}
