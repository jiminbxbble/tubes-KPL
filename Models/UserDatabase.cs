using BCrypt.Net;

namespace MonTrack.Auth.Models
{
    public static class UserDatabase
    {
        // Simulasi database user (pengganti database sungguhan)
        private static readonly List<User> _users = new List<User>
        {
            new User(1, "rosa@email.com", BCrypt.Net.BCrypt.HashPassword("password123"), true),
            new User(2, "raissha@email.com", BCrypt.Net.BCrypt.HashPassword("mypassword"), false),
            new User(3, "putri@email.com", BCrypt.Net.BCrypt.HashPassword("putri1234"), true),
        };

        public static List<User> GetUsers()
        {
            return _users;
        }

        public static User? FindByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }

        public static void AddUser(User user)
        {
            _users.Add(user);
        }
    }
}