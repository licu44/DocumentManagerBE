using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Models
{
    public class UserStatus
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("FeedbackStatus")]
        public int FeddbackId { get; set; }

        [ForeignKey("AuthorizationStatus")]
        public int AuthorizationId { get; set; }

        [ForeignKey("EngineeringStatus")]
        public int EngineeringId { get; set; }

        public virtual User User { get; set; }
        public virtual FeedbackStatus FeedbackStatus { get; set; }
        public virtual AuthorizationStatus AuthorizationStatus { get; set; }
        public virtual EngineeringStatus EngineeringStatus { get; set; }
    }
}
