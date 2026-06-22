using MonTrack.Auth.Models;
using MonTrack.Auth.Services;
using MonTrack.Auth.States;
using MonTrack.Auth.StateMachine;

namespace MonTrack.Auth.Api
{
    // Simulasi API Response
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }

        public static ApiResponse<T> Success(T data, string message, int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = data,
                IsSuccess = true
            };
        }
         
        public static ApiResponse<T> Fail(string message, int statusCode = 400)
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = default,
                IsSuccess = false
            };
        }
    }

    public class AuthApiSimulator
    {
        private readonly AuthService _authService;

        public AuthApiSimulator()
        {
            _authService = new AuthService();
        }

        // POST /api/auth/register
        public ApiResponse<User> Register(string email, string password)
        {
            Console.WriteLine("\n[API] POST /api/auth/register");
            try
            {
                var user = _authService.Register(email, password);
                return ApiResponse<User>.Success(user, "Registrasi berhasil.", 201);
            }
            catch (Exception ex)
            {
                return ApiResponse<User>.Fail(ex.Message);
            }
        }

        // POST /api/auth/login
        public ApiResponse<string> Login(string email, string password)
        {
            Console.WriteLine("\n[API] POST /api/auth/login");
            try
            {
                var context = new AuthContext
                {
                    Email = email,
                    Password = password
                };

                var stateMachine = new AuthStateMachine(context);
                stateMachine.Run();

                if (stateMachine.IsSuccess())
                    return ApiResponse<string>.Success("LOGIN_SUCCESS", context.Message);
                else
                    return ApiResponse<string>.Fail(context.Message, 401);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail(ex.Message, 400);
            }
        }

        // POST /api/auth/generate-2fa
        public ApiResponse<string> GenerateTwoFactorCode(string email)
        {
            Console.WriteLine("\n[API] POST /api/auth/generate-2fa");
            try
            {
                var code = _authService.GenerateTwoFactorCode(email);
                return ApiResponse<string>.Success(code, "Kode 2FA berhasil digenerate.");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail(ex.Message);
            }
        }

        // POST /api/auth/verify-2fa
        public ApiResponse<string> VerifyTwoFactor(string email, string password, string code)
        {
            Console.WriteLine("\n[API] POST /api/auth/verify-2fa");
            try
            {
                var user = UserDatabase.FindByEmail(email);
                if (user == null)
                    return ApiResponse<string>.Fail("User tidak ditemukan.", 404);

                var context = new AuthContext
                {
                    Email = email,
                    Password = password,
                    TwoFactorCode = code
                };

                var stateMachine = new AuthStateMachine(context);
                stateMachine.Run();

                if (stateMachine.IsSuccess())
                    return ApiResponse<string>.Success("LOGIN_2FA_SUCCESS", context.Message);
                else
                    return ApiResponse<string>.Fail(context.Message, 401);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail(ex.Message, 400);
            }
        }

        // POST /api/auth/enable-2fa
        public ApiResponse<string> Enable2FA(string email)
        {
            Console.WriteLine("\n[API] POST /api/auth/enable-2fa");
            try
            {
                _authService.Enable2FA(email);
                return ApiResponse<string>.Success("2FA_ENABLED", "2FA berhasil diaktifkan.");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail(ex.Message);
            }
        }
    }
}