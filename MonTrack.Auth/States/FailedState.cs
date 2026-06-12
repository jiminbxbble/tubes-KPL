namespace MonTrack.Auth.States
{
    public class FailedState : IAuthState
    {
        public string StateName => "Failed";

        public void Handle(AuthContext context)
        {
            Console.WriteLine($"[State: Failed] Autentikasi gagal: {context.Message}");

            context.IsSuccess = false;
            context.NextState = null; // State terakhir, tidak ada state berikutnya
        }
    }
}