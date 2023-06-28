namespace DocumentManager.Data
{
    public class AdminUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserStatusDTO UserStatus { get; set; } = new UserStatusDTO(); // Initialize a default instance of UserStatusDTO
    }

    public class UserStatusDTO
    {
        public int? FeedbackId { get; set; } = null; // Nullable int, default is null
        public int? AuthorizationId { get; set; } = null; // Nullable int, default is null
        public int? EngineeringId { get; set; } = null; // Nullable int, default is null
    }
}
