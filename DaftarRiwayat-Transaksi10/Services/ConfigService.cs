using DaftarRiwayat_Transaksi10.Configs;
using DaftarRiwayat_Transaksi10.Models;
using System;
using System.IO;
using System.Text.Json;

namespace DaftarRiwayat_Transaksi10.Services
{
    public static class ConfigService
    {
        private const string FilePath = "config.json";

        public static AppConfig LoadConfig()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string jsonString = File.ReadAllText(FilePath);
                    var config = JsonSerializer.Deserialize<AppConfig>(jsonString);
                    return config ?? new AppConfig();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($">> Gagal memuat config: {ex.Message}. Menggunakan default.");
            }

            return new AppConfig();
        }
    }
}