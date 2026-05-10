using MonTrack.Auth.Api;
using MonTrack.Auth.Models;

var api = new AuthApiSimulator();

Console.WriteLine("╔════════════════════════════════════╗");
Console.WriteLine("║     MonTrack Authentication        ║");
Console.WriteLine("╚════════════════════════════════════╝");

bool running = true;
while (running)
{
    Console.WriteLine("\n===== MENU =====");
    Console.WriteLine("1. Register");
    Console.WriteLine("2. Login");
    Console.WriteLine("3. Login dengan 2FA");
    Console.WriteLine("4. Aktifkan 2FA");
    Console.WriteLine("0. Keluar");
    Console.Write("Pilih menu: ");

    string? pilihan = Console.ReadLine();

    switch (pilihan)
    {
        case "1":
            Console.Write("Email: ");
            string? regEmail = Console.ReadLine();
            Console.Write("Password: ");
            string? regPassword = Console.ReadLine();

            var regResult = api.Register(regEmail!, regPassword!);
            Console.WriteLine($"\nStatus: {regResult.StatusCode} | {regResult.Message}");
            break;

        case "2":
            Console.Write("Email: ");
            string? loginEmail = Console.ReadLine();
            Console.Write("Password: ");
            string? loginPassword = Console.ReadLine();

            var loginResult = api.Login(loginEmail!, loginPassword!);
            Console.WriteLine($"\nStatus: {loginResult.StatusCode} | {loginResult.Message}");
            break;

        case "3":
            Console.Write("Email: ");
            string? tfaEmail = Console.ReadLine();
            Console.Write("Password: ");
            string? tfaPassword = Console.ReadLine();

            // Generate kode 2FA dulu
            var genResult = api.GenerateTwoFactorCode(tfaEmail!);
            Console.WriteLine($"\nStatus: {genResult.StatusCode} | {genResult.Message}");

            Console.Write("\nMasukkan kode 2FA: ");
            string? tfaCode = Console.ReadLine();

            var twoFAResult = api.VerifyTwoFactor(tfaEmail!, tfaPassword!, tfaCode!);
            Console.WriteLine($"\nStatus: {twoFAResult.StatusCode} | {twoFAResult.Message}");
            break;

        case "4":
            Console.Write("Email: ");
            string? enableEmail = Console.ReadLine();

            var enableResult = api.Enable2FA(enableEmail!);
            Console.WriteLine($"\nStatus: {enableResult.StatusCode} | {enableResult.Message}");
            break;

        case "0":
            running = false;
            Console.WriteLine("Sampai jumpa!");
            break;

        default:
            Console.WriteLine("Pilihan tidak valid!");
            break;
    }
}