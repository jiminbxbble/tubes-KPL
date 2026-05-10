using MonTrack.Auth.Models;

namespace MonTrack.Auth.States
{
    public class AuthenticatedState : IAuthState
    {
        public string StateName => "Authenticated";

        private readonly User _user;

        public AuthenticatedState(User user)
        {
            _user = user;
        }

        public void Handle(AuthContext context)
        {
            Console.WriteLine("[State: Authenticated] Pengguna berhasil login!");

            // Design by Contract - Invariant
            // Contracts.AuthContracts.CheckUserInvariant(_user.Email, _user.PasswordHash);

            _user.IsAuthenticated = true;

            context.IsSuccess = true;
            context.Message = $"Selamat datang, {_user.Email}! Login berhasil.";
            context.NextState = null; // State terakhir, tidak ada state berikutnya
        }
    }
}