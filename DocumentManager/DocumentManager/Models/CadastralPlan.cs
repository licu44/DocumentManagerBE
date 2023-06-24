using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Models
{
    public class CadastralPlan
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        public string Surface { get; set; }
        public virtual User User { get; set; }
    }
}
