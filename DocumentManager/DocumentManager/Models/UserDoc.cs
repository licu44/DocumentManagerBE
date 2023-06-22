using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Models
{
    public class UserDoc
    {
        public int UserId { get; set; }
        public int TypeId { get; set; }

        public DateTime? CreationDate { get; set; }
        public string Status { get; set; }

        public virtual User User { get; set; }
        public virtual DocumentType Type { get; set; }
    }
}
