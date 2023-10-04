using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Models
{
    public class GenerateDocType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Type { get; set; }
        public bool Restricted { get; set; }
        public virtual ICollection<UserGeneratedDoc> UserGeneratedDocs { get; set; }

    }
}
