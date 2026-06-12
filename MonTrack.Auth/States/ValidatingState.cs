using MonTrack.Auth.Models;

namespace MonTrack.Auth.States
{
    public class ValidatingState : IAuthState
    {
        public string StateName => "Validating";

        private readonly List<User> _users;

        public ValidatingState(List<User>? users = null)
        {
            _users = users ?? UserDatabase.GetUsers();
        }

        public void Handle(AuthContext context)
        {
            Console.WriteLine("[State: Validating] Memvalidasi kredensial...");

            var user = _users.FirstOrDefault(u => u.Email == context.Email);

            if (user == null)
            {
                context.Message = "Email tidak ditemukan.";
                context.IsSuccess = false;
                context.NextState = new FailedState();
                return;
            }

            // Cek password menggunakan BCrypt
            bool passwordValid = BCrypt.Net.BCrypt.Verify(context.Password, user.PasswordHash);

            if (!passwordValid)
            {
                context.Message = "Password salah.";
                context.IsSuccess = false;
                context.NextState = new FailedState();
                return;
            }

            context.Message = "Kredensial valid.";

            // Cek apakah user mengaktifkan 2FA
            if (user.Is2FAEnabled)
            {
                context.NextState = new Awaiting2FAState(user);
            }
            else
            {
                context.IsSuccess = true;
                context.NextState = new AuthenticatedState(user);
            }
        }
    }
}