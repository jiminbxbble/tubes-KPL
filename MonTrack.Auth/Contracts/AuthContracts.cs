namespace MonTrack.Auth.Contracts
{
    public static class AuthContracts
    {
        // Precondition: pastikan email tidak kosong dan valid
        public static void RequireValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email tidak boleh kosong.");
            if (!email.Contains("@") || !email.Contains("."))
                throw new ArgumentException("Format email tidak valid.");
        }

        // Precondition: pastikan password tidak kosong dan minimal 8 karakter
        public static void RequireValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password tidak boleh kosong.");
            if (password.Length < 8)
                throw new ArgumentException("Password minimal 8 karakter.");
        }

        // Precondition: pastikan kode 2FA tidak kosong dan 6 digit
        public static void RequireValid2FACode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Kode 2FA tidak boleh kosong.");
            if (code.Length != 6 || !code.All(char.IsDigit))
                throw new ArgumentException("Kode 2FA harus 6 digit angka.");
        }

        // Postcondition: pastikan user tidak null setelah register
        public static void EnsureUserCreated(object? user)
        {
            if (user == null)
                throw new InvalidOperationException("User gagal dibuat.");
        }

        // Invariant: pastikan email user selalu terisi
        public static void CheckUserInvariant(string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidOperationException("Invariant gagal: Email user tidak boleh kosong.");
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new InvalidOperationException("Invariant gagal: PasswordHash tidak boleh kosong.");
        }
    }
}