using NUnit.Framework;
using MonTrack.Auth.Services;
using MonTrack.Auth.States;
using MonTrack.Auth.StateMachine;
using MonTrack.Auth.Models;
using System.Diagnostics;

namespace MonTrack.Auth.Tests
{
    [TestFixture]
    public class PerformanceTests
    {
        private AuthService _authService;
        private const int ITERASI = 100;

        [SetUp]
        public void Setup()
        {
            UserDatabase.ResetToDefault();
            _authService = new AuthService();
        }

        [Test]
        public void Performance_Login_TanpaA2FA_100Iterasi()
        {
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < ITERASI; i++)
            {
                var context = new AuthContext
                {
                    Email = "raissha@email.com",
                    Password = "mypassword"
                };
                var sm = new AuthStateMachine(context);
                sm.Run();
            }

            stopwatch.Stop();
            long totalMs = stopwatch.ElapsedMilliseconds;
            double rataRataMs = (double)totalMs / ITERASI;

            Console.WriteLine($"[Performance] Login Tanpa 2FA");
            Console.WriteLine($"Total waktu ({ITERASI} iterasi): {totalMs} ms");
            Console.WriteLine($"Rata-rata per login: {rataRataMs:F2} ms");

            // Batas maksimal 200ms per login (BCrypt membutuhkan waktu untuk verifikasi)
            Assert.That(rataRataMs, Is.LessThan(200),
                $"Rata-rata login terlalu lambat: {rataRataMs:F2} ms");
        }

        [Test]
        public void Performance_Register_100Iterasi()
        {
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < ITERASI; i++)
            {
                _authService.Register($"perfuser{i}@email.com", "password123");
            }

            stopwatch.Stop();
            long totalMs = stopwatch.ElapsedMilliseconds;
            double rataRataMs = (double)totalMs / ITERASI;

            Console.WriteLine($"[Performance] Register User Baru");
            Console.WriteLine($"Total waktu ({ITERASI} iterasi): {totalMs} ms");
            Console.WriteLine($"Rata-rata per register: {rataRataMs:F2} ms");

            // Batas maksimal 200ms per register (BCrypt hashing membutuhkan waktu)
            Assert.That(rataRataMs, Is.LessThan(200),
                $"Rata-rata register terlalu lambat: {rataRataMs:F2} ms");
        }

        [Test]
        public void Performance_Generate2FA_100Iterasi()
        {
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < ITERASI; i++)
            {
                _authService.GenerateTwoFactorCode("rosa@email.com");
            }

            stopwatch.Stop();
            long totalMs = stopwatch.ElapsedMilliseconds;
            double rataRataMs = (double)totalMs / ITERASI;

            Console.WriteLine($"[Performance] Generate Kode 2FA");
            Console.WriteLine($"Total waktu ({ITERASI} iterasi): {totalMs} ms");
            Console.WriteLine($"Rata-rata per generate: {rataRataMs:F2} ms");

            // Batas maksimal 10ms per generate (hanya random number generation)
            Assert.That(rataRataMs, Is.LessThan(10),
                $"Rata-rata generate 2FA terlalu lambat: {rataRataMs:F2} ms");
        }

        [Test]
        public void Performance_Login_Dengan2FA_100Iterasi()
        {
            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < ITERASI; i++)
            {
                var code = _authService.GenerateTwoFactorCode("rosa@email.com");
                var context = new AuthContext
                {
                    Email = "rosa@email.com",
                    Password = "password123",
                    TwoFactorCode = code
                };
                var sm = new AuthStateMachine(context);
                sm.Run();
            }

            stopwatch.Stop();
            long totalMs = stopwatch.ElapsedMilliseconds;
            double rataRataMs = (double)totalMs / ITERASI;

            Console.WriteLine($"[Performance] Login Dengan 2FA");
            Console.WriteLine($"Total waktu ({ITERASI} iterasi): {totalMs} ms");
            Console.WriteLine($"Rata-rata per login+2FA: {rataRataMs:F2} ms");

            // Batas maksimal 200ms per login dengan 2FA
            Assert.That(rataRataMs, Is.LessThan(200),
                $"Rata-rata login 2FA terlalu lambat: {rataRataMs:F2} ms");
        }
    }
}