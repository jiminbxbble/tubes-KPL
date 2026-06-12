namespace MonTrack.Auth.States
{
    public interface IAuthState
    {
        string StateName { get; }
        void Handle(AuthContext context);
    }

    public class AuthContext
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string TwoFactorCode { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public IAuthState? NextState { get; set; }
    }
}