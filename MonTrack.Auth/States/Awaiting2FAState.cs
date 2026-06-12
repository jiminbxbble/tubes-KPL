using MonTrack.Auth.Contracts;
using MonTrack.Auth.Models;

namespace MonTrack.Auth.States
{
    public class Awaiting2FAState : IAuthState
    {
        public string StateName => "Awaiting2FA";

        private readonly User _user;

        public Awaiting2FAState(User user)
        {
            _user = user;
        }

        public void Handle(AuthContext context)
        {
            Console.WriteLine("[State: Awaiting2FA] Menunggu verifikasi kode 2FA...");

            // Design by Contract - Precondition
            AuthContracts.RequireValid2FACode(context.TwoFactorCode);

            // Cek apakah kode 2FA cocok dan belum expired
            if (_user.TwoFactorCode == null || _user.TwoFactorExpiry == null)
            {
                context.Message = "Kode 2FA belum di-generate.";
                context.IsSuccess = false;
                context.NextState = new FailedState();
                return;
            }

            if (DateTime.Now > _user.TwoFactorExpiry)
            {
                context.Message = "Kode 2FA sudah expired.";
                context.IsSuccess = false;
                context.NextState = new FailedState();
                return;
            }

            if (context.TwoFactorCode != _user.TwoFactorCode)
            {
                context.Message = "Kode 2FA salah.";
                context.IsSuccess = false;
                context.NextState = new FailedState();
                return;
            }

            context.Message = "Verifikasi 2FA berhasil.";
            context.IsSuccess = true;
            context.NextState = new AuthenticatedState(_user);
        }
    }
}