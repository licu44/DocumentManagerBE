using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Models
{
    public class User
    {
        [Key]
        public int Id {get; set;}
        public string Username { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set; }
        public string Email { get; set;}
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        public byte[] PasswordHash { get; set;}
        public byte[] PasswordSalt { get; set; }

    }
}
