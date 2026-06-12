using MonTrack.Auth.Contracts;
using MonTrack.Auth.Models;

namespace MonTrack.Auth.Services
{
    public class AuthService
    {
        private readonly TwoFactorService _twoFactorService;

        public AuthService()
        {
            _twoFactorService = new TwoFactorService();
        }

        // Register user baru
        public User Register(string email, string password)
        {
            // Design by Contract - Precondition
            AuthContracts.RequireValidEmail(email);
            AuthContracts.RequireValidPassword(password);

            // Cek apakah email sudah terdaftar
            var existingUser = UserDatabase.FindByEmail(email);
            if (existingUser != null)
                throw new InvalidOperationException("Email sudah terdaftar.");

            // Hash password menggunakan BCrypt
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            // Buat user baru
            var newUser = new User(
                UserDatabase.GetUsers().Count + 1,
                email,
                passwordHash,
                false
            );

            // Design by Contract - Postcondition
            AuthContracts.EnsureUserCreated(newUser);

            // Simpan ke database
            UserDatabase.AddUser(newUser);

            // Design by Contract - Invariant
            AuthContracts.CheckUserInvariant(newUser.Email, newUser.PasswordHash);

            Console.WriteLine($"[Auth Service] User {email} berhasil didaftarkan!");
            return newUser;
        }

        // Generate kode 2FA untuk user
        public string GenerateTwoFactorCode(string email)
        {
            // Design by Contract - Precondition
            AuthContracts.RequireValidEmail(email);

            var user = UserDatabase.FindByEmail(email);
            if (user == null)
                throw new InvalidOperationException("User tidak ditemukan.");

            return _twoFactorService.GenerateCode(user);
        }

        // Enable 2FA untuk user
        public void Enable2FA(string email)
        {
            var user = UserDatabase.FindByEmail(email);
            if (user == null)
                throw new InvalidOperationException("User tidak ditemukan.");

            user.Is2FAEnabled = true;
            Console.WriteLine($"[Auth Service] 2FA diaktifkan untuk {email}.");
        }
    }
}