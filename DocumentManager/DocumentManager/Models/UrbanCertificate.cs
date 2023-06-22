using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Models
{
    public class UrbanCertificate
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        public string UserAdress { get; set; }
        [Required]
        public string ProjectAdress { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string ProjectType { get; set; }
        public virtual User User { get; set; }

    }
}
