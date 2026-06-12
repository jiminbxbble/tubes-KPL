using MonTrack.Auth.Contracts;

namespace MonTrack.Auth.States
{
    public class IdleState : IAuthState
    {
        public string StateName => "Idle";

        public void Handle(AuthContext context)
        {
            Console.WriteLine("[State: Idle] Memulai proses autentikasi...");

            // Design by Contract - Precondition
            AuthContracts.RequireValidEmail(context.Email);
            AuthContracts.RequireValidPassword(context.Password);

            context.Message = "Kredensial diterima, memulai validasi...";

            // Pindah ke state berikutnya
            context.NextState = new ValidatingState();
        }
    }
}