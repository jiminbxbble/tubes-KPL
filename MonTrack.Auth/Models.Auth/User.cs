namespace MonTrack.Auth.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool Is2FAEnabled { get; set; } = false;
        public string? TwoFactorCode { get; set; }
        public DateTime? TwoFactorExpiry { get; set; }
        public bool IsAuthenticated { get; set; } = false;

        public User(int id, string email, string passwordHash, bool is2FAEnabled = false)
        {
            Id = id;
            Email = email;
            PasswordHash = passwordHash;
            Is2FAEnabled = is2FAEnabled;
        }
    }
}