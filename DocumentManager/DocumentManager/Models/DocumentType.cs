using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Models
{
    public class DocumentType
    {
        [Key]
        public int Id { get; set; }
        public string DocName { get; set; }
        public virtual ICollection<UserDoc> UserDocs { get; set; }

    }
}
