using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManager.Models
{
    public class IdCard
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string CNP { get; set; }

        [Required]
        public string Series { get; set; }

        [Required]
        public string Number { get; set; }


        public virtual User User { get; set; }
    }

}
