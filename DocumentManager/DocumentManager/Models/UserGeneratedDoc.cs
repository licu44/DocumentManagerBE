using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager.Models
{
    public class UserGeneratedDoc
    {
        [ForeignKey("User")]
        public int UserId { get; set; }
        public int TypeId { get; set; }
        [Required]
        public DateTime? CreationDate { get; set; }
        [Required]
        public string WordDocumentPath { get; set; }

        public virtual User User { get; set; }
        public virtual GenerateDocType Type { get; set; }

    }
}
