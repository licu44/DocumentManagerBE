using DocumentManager.Models;
using System.ComponentModel.DataAnnotations;

namespace DocumentManager.Data
{
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        public string Role { get; set; }

    }
}
