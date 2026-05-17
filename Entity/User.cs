namespace KhadeFarm_Web_API.Entity
{
    public class User
    {
        public int UserId { get; set; }                 // Primary Key
        public string Username { get; set; } = string.Empty;  // Unique username
        public string Email { get; set; } = string.Empty;     // Unique email
        public string Password { get; set; } = string.Empty;  // Hashed password
        public string FullName { get; set; } = string.Empty;  // Optional full name

        public int RoleId { get; set; } = 101;     // FK to Roles table
        public string RoleValue { get; set; } = "CUST"; // Role name/value

        public bool IsActive { get; set; } = true;      // Active flag

        public string CreatedBy { get; set; } = "System";     // Audit: who created
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Audit: when created

        public string? ModifiedBy { get; set; } = "System";     // Audit: who modified
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;   // Audit: when modified
    }


    public class UserLoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
