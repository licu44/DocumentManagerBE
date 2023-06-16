using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager.Models
{
    public class UserRole
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
    }
}