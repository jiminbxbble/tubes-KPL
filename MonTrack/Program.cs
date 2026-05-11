using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MonTrack.Models;
using MonTrack.Services;
using MonTrack.Tests;

namespace MonTrack
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                   MONTRACK - FR-08                         ║");
            Console.WriteLine("║           Ekspor Data Transaksi (Export Feature)           ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║  Teknik yang diimplementasikan:                           ║");
            Console.WriteLine("║  1. Design by Contract (DbC) - Pre/Post conditions         ║");
            Console.WriteLine("║  2. Code Reuse / Library (CsvHelper)                       ║");
            Console.WriteLine("║  3. API Internal (ExportApiService)                        ║");
            Console.WriteLine("║  4. Unit Testing (CLO4 - 70% nilai)                       ║");
            Console.WriteLine("║  5. Performance Testing (CLO2 - Performance)               ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            // Menu utama
            while (true)
            {
                Console.WriteLine("Pilih menu:");
                Console.WriteLine("1. Demo Ekspor Data (Basic Usage)");
                Console.WriteLine("2. Jalankan Unit Tests");
                Console.WriteLine("3. Jalankan Performance Tests");
                Console.WriteLine("4. Exit");
                Console.Write("\nMasukkan pilihan (1-4): ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await RunBasicDemo();
                        break;
                    case "2":
                        await RunUnitTests();
                        break;
                    case "3":
                        await RunPerformanceTests();
                        break;
                    case "4":
                        Console.WriteLine("\nTerima kasih! Sampai jumpa.");
                        return;
                    default:
                        Console.WriteLine("Pilihan tidak valid\n");
                        break;
                }
            }
        }

        /// <summary>
        /// Demo penggunaan dasar fitur ekspor
        /// </summary>
        static async Task RunBasicDemo()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              DEMO: BASIC USAGE - EXPORT DATA               ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            try
            {
                // Buat sample data
                var transactions = new List<Transaction>
                {
                    new Transaction(1, DateTime.Now.AddDays(-10), 5000000, "Gaji", "Gaji bulanan Mei 2026"),
                    new Transaction(2, DateTime.Now.AddDays(-9), 500000, "Makanan", "Makan di restoran"),
                    new Transaction(3, DateTime.Now.AddDays(-8), 150000, "Transportasi", "Bensin mobil"),
                    new Transaction(4, DateTime.Now.AddDays(-7), 2000000, "Investasi", "Beli saham"),
                    new Transaction(5, DateTime.Now.AddDays(-6), 350000, "Utilitas", "Bayar listrik"),
                    new Transaction(6, DateTime.Now.AddDays(-5), 750000, "Freelance", "Proyek web development"),
                    new Transaction(7, DateTime.Now.AddDays(-4), 250000, "Entertainment", "Bioskop"),
                    new Transaction(8, DateTime.Now, 1200000, "Bonus", "Bonus kinerja bulan Mei")
                }!;

                Console.WriteLine("Sample transactions yang akan diekspor:");
                Console.WriteLine("───────────────────────────────────────────────────────────");
                foreach (var tx in transactions)
                {
                    Console.WriteLine($"ID {tx.Id}: {tx.Date:dd/MM/yyyy} | {tx.Amount:N0} | {tx.Category} | {tx.Description}");
                }

                // Inisialisasi service
                var service = new ExportApiService();

                // Tampilkan format yang tersedia
                Console.WriteLine("\n───────────────────────────────────────────────────────────");
                Console.WriteLine("Format eksporter yang tersedia:");
                foreach (var format in service.GetSupportedFormats())
                {
                    Console.WriteLine($"  • {format.ToUpper()}");
                }

                // Lakukan ekspor
                Console.WriteLine("\n───────────────────────────────────────────────────────────");
                string outputPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "montrack_export.csv");
                await service.ExecuteExport("csv", transactions, outputPath);

                // Verifikasi hasil
                if (System.IO.File.Exists(outputPath))
                {
                    var fileInfo = new System.IO.FileInfo(outputPath);
                    Console.WriteLine($"✓ File berhasil dibuat:");
                    Console.WriteLine($"  Path: {outputPath}");
                    Console.WriteLine($"  Size: {fileInfo.Length} bytes");
                    Console.WriteLine("\nPreview isi file (5 baris pertama):");
                    var lines = System.IO.File.ReadAllLines(outputPath);
                    for (int i = 0; i < Math.Min(5, lines.Length); i++)
                    {
                        Console.WriteLine($"  {lines[i]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Error: {ex.Message}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Jalankan unit tests
        /// </summary>
        static async Task RunUnitTests()
        {
            Console.WriteLine();
            var testSuite = new ExportApiServiceTests();
            await testSuite.RunAllTests();
            Console.WriteLine();
        }

        /// <summary>
        /// Jalankan performance tests
        /// </summary>
        static async Task RunPerformanceTests()
        {
            Console.WriteLine();
            var perfTest = new PerformanceTest();
            await perfTest.RunAllPerformanceTests();
            Console.WriteLine();
        }
    }
}
