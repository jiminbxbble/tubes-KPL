using NUnit.Framework;
using MonTrack.Auth.Services;
using MonTrack.Auth.States;
using MonTrack.Auth.StateMachine;
using MonTrack.Auth.Contracts;
using MonTrack.Auth.Models;

namespace MonTrack.Auth.Tests
{
    [TestFixture]
    public class UnitTests
    {
        private AuthService _authService;

        [SetUp]
        public void Setup()
        {
            _authService = new AuthService();
        }

        // ===== TEST AUTHCONTRACTS =====

        [Test]
        public void RequireValidEmail_EmailKosong_ThrowException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                AuthContracts.RequireValidEmail(""));
            Assert.That(ex.Message, Does.Contain("kosong"));
        }

        [Test]
        public void RequireValidEmail_EmailTidakValid_ThrowException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                AuthContracts.RequireValidEmail("emailtidakvalid"));
            Assert.That(ex.Message, Does.Contain("tidak valid"));
        }

        [Test]
        public void RequireValidEmail_EmailValid_TidakThrowException()
        {
            Assert.DoesNotThrow(() =>
                AuthContracts.RequireValidEmail("rosa@email.com"));
        }

        [Test]
        public void RequireValidPassword_PasswordKosong_ThrowException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                AuthContracts.RequireValidPassword(""));
            Assert.That(ex.Message, Does.Contain("kosong"));
        }

        [Test]
        public void RequireValidPassword_PasswordKurangDari8_ThrowException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                AuthContracts.RequireValidPassword("abc"));
            Assert.That(ex.Message, Does.Contain("minimal 8"));
        }

        [Test]
        public void RequireValid2FACode_KodeTidak6Digit_ThrowException()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
                AuthContracts.RequireValid2FACode("123"));
            Assert.That(ex.Message, Does.Contain("6 digit"));
        }

        [Test]
        public void RequireValid2FACode_KodeValid_TidakThrowException()
        {
            Assert.DoesNotThrow(() =>
                AuthContracts.RequireValid2FACode("123456"));
        }

        // ===== TEST REGISTER =====

        [Test]
        public void Register_UserBaru_Berhasil()
        {
            var user = _authService.Register("newuser1@email.com", "password123");
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Email, Is.EqualTo("newuser1@email.com"));
        }

        [Test]
        public void Register_EmailSudahAda_ThrowException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                _authService.Register("rosa@email.com", "password123"));
        }

        // ===== TEST LOGIN TANPA 2FA =====

        [Test]
        public void Login_KredensialValid_Berhasil()
        {
            var context = new AuthContext
            {
                Email = "raissha@email.com",
                Password = "mypassword"
            };
            var sm = new AuthStateMachine(context);
            sm.Run();
            Assert.That(sm.IsSuccess(), Is.True);
        }

        [Test]
        public void Login_PasswordSalah_Gagal()
        {
            var context = new AuthContext
            {
                Email = "raissha@email.com",
                Password = "passwordsalah"
            };
            var sm = new AuthStateMachine(context);
            sm.Run();
            Assert.That(sm.IsSuccess(), Is.False);
        }

        [Test]
        public void Login_EmailTidakDitemukan_Gagal()
        {
            var context = new AuthContext
            {
                Email = "tidakada@email.com",
                Password = "password123"
            };
            var sm = new AuthStateMachine(context);
            sm.Run();
            Assert.That(sm.IsSuccess(), Is.False);
        }

        // ===== TEST LOGIN DENGAN 2FA =====

        [Test]
        public void Login_Dengan2FA_KodeValid_Berhasil()
        {
            // Generate kode 2FA dulu
            var code = _authService.GenerateTwoFactorCode("rosa@email.com");

            var context = new AuthContext
            {
                Email = "rosa@email.com",
                Password = "password123",
                TwoFactorCode = code
            };
            var sm = new AuthStateMachine(context);
            sm.Run();
            Assert.That(sm.IsSuccess(), Is.True);
        }

        [Test]
        public void Login_Dengan2FA_KodeSalah_Gagal()
        {
            _authService.GenerateTwoFactorCode("rosa@email.com");

            var context = new AuthContext
            {
                Email = "rosa@email.com",
                Password = "password123",
                TwoFactorCode = "000000"
            };
            var sm = new AuthStateMachine(context);
            sm.Run();
            Assert.That(sm.IsSuccess(), Is.False);
        }
    }
}