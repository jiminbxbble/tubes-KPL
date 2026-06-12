using MonTrack.Auth.States;

namespace MonTrack.Auth.StateMachine

{
    public class AuthStateMachine
    {
        private IAuthState _currentState;
        private readonly AuthContext _context;

        public AuthStateMachine(AuthContext context)
        {
            _context = context;
            _currentState = new IdleState(); // State awal selalu Idle
        }

        public void Run()
        {
            Console.WriteLine("=== Memulai State Machine Autentikasi ===");

            while (_currentState != null)
            {
                Console.WriteLine($"\n>> Current State: {_currentState.StateName}");
                _currentState.Handle(_context);
                _currentState = _context.NextState!;
            }

            Console.WriteLine("\n=== State Machine Selesai ===");
            Console.WriteLine($"Status: {(_context.IsSuccess ? "BERHASIL" : "GAGAL")}");
            Console.WriteLine($"Pesan: {_context.Message}");
        }

        public string GetCurrentStateName()
        {
            return _currentState?.StateName ?? "Selesai";
        }

        public bool IsSuccess()
        {
            return _context.IsSuccess;
        }
    }
}