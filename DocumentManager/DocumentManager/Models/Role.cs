using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
}
