using MonTrack.Auth.Models;

namespace MonTrack.Auth.Services
{
    public class TwoFactorService
    {
        private readonly Random _random = new Random();

        // Generate kode 2FA 6 digit dan simpan ke user
        public string GenerateCode(User user)
        {
            string code = _random.Next(100000, 999999).ToString();
            user.TwoFactorCode = code;
            user.TwoFactorExpiry = DateTime.Now.AddMinutes(5); // expired dalam 5 menit

            Console.WriteLine($"[2FA Service] Kode 2FA untuk {user.Email}: {code}");
            Console.WriteLine($"[2FA Service] Kode berlaku hingga: {user.TwoFactorExpiry}");

            return code;
        }

        // Validasi kode 2FA
        public bool ValidateCode(User user, string code)
        {
            if (user.TwoFactorCode == null || user.TwoFactorExpiry == null)
                return false;

            if (DateTime.Now > user.TwoFactorExpiry)
                return false;

            return user.TwoFactorCode == code;
        }

        // Reset kode 2FA setelah digunakan
        public void ResetCode(User user)
        {
            user.TwoFactorCode = null;
            user.TwoFactorExpiry = null;
        }
    }
}