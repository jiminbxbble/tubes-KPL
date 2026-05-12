using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using MonTrack.Models;
using MonTrack.Services;

namespace MonTrack
{
    class Program
    {
        static async Task Main(string[] args) // CLI buat nyoba nyoba aja bang
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                   MONTRACK - EXPORT DEMO                   ║");
            Console.WriteLine("║          CSV & PDF Export with Performance Testing         ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            // Persiapan data dummy
            int recordCount = 1000; // Reduced untuk testing yang lebih cepat
            var dummyData = GenerateDummyTransactions(recordCount);
            var exportService = new ExportApiService();

            Console.WriteLine("Pilih format ekspor:");
            Console.WriteLine("1. CSV Export");
            Console.WriteLine("2. PDF Export");
            Console.WriteLine("3. Both (CSV + PDF)");
            Console.WriteLine("4. Performance Test (10,000 records to CSV)");
            Console.Write("\nMasukkan pilihan (1-4): ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ExportCSV(dummyData, exportService);
                    break;
                case "2":
                    await ExportPDF(dummyData, exportService);
                    break;
                case "3":
                    await ExportCSV(dummyData, exportService);
                    await ExportPDF(dummyData, exportService);
                    break;
                case "4":
                    await PerformanceTest(exportService);
                    break;
                default:
                    Console.WriteLine("Pilihan tidak valid!");
                    break;
            }

            Console.WriteLine("\nTekan tombol apa saja untuk keluar...");
            Console.ReadKey();
        }

        static async Task ExportCSV(List<Transaction> data, ExportApiService service)
        {
            Console.WriteLine("\n--- CSV Export ---");
            string exportFolder = "ExportResults";
            Directory.CreateDirectory(exportFolder);
            string outputPath = Path.Combine(exportFolder, $"export_data_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await service.ExecuteExport("CSV", data, outputPath);
                stopwatch.Stop();

                var fileInfo = new FileInfo(outputPath);
                Console.WriteLine($"✓ CSV Export Berhasil!");
                Console.WriteLine($"  File: {Path.GetFullPath(outputPath)}");
                Console.WriteLine($"  Ukuran: {fileInfo.Length / 1024.0:F2} KB");
                Console.WriteLine($"  Waktu: {stopwatch.ElapsedMilliseconds} ms");
                Console.WriteLine($"  Records: {data.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ CSV Export Gagal: {ex.Message}");
            }
        }

        static async Task ExportPDF(List<Transaction> data, ExportApiService service)
        {
            Console.WriteLine("\n--- PDF Export ---");
            string exportFolder = "ExportResults";
            Directory.CreateDirectory(exportFolder);
            string outputPath = Path.Combine(exportFolder, $"export_data_{DateTime.Now:yyyyMMdd_HHmmss_fff}.pdf");
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await service.ExecuteExport("PDF", data, outputPath);
                stopwatch.Stop();

                var fileInfo = new FileInfo(outputPath);
                Console.WriteLine($"✓ PDF Export Berhasil!");
                Console.WriteLine($"  File: {Path.GetFullPath(outputPath)}");
                Console.WriteLine($"  Ukuran: {fileInfo.Length / 1024.0:F2} KB");
                Console.WriteLine($"  Waktu: {stopwatch.ElapsedMilliseconds} ms");
                Console.WriteLine($"  Records: {data.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ PDF Export Gagal: {ex.Message}");
            }
        }

        static async Task PerformanceTest(ExportApiService service)
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║          PERFORMANCE TEST - 10,000 RECORDS (CSV)            ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

            int recordCount = 10000;
            var dummyData = GenerateDummyTransactions(recordCount);
            string exportFolder = "ExportResults";
            Directory.CreateDirectory(exportFolder);
            string outputPath = Path.Combine(exportFolder, $"performance_test_export_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
            var stopwatch = Stopwatch.StartNew();

            try
            {
                Console.WriteLine($"Mengekspor {recordCount} data ke {outputPath}...");
                await service.ExecuteExport("CSV", dummyData, outputPath);
                stopwatch.Stop();

                var fileInfo = new FileInfo(outputPath);
                double throughput = (recordCount / (stopwatch.ElapsedMilliseconds / 1000.0));

                Console.WriteLine("\n--- Hasil Pengujian Performa ---");
                Console.WriteLine($"Jumlah Data     : {recordCount:N0} transaksi");
                Console.WriteLine($"Waktu Eksekusi  : {stopwatch.ElapsedMilliseconds} ms");
                Console.WriteLine($"Throughput      : {throughput:F0} records/second");
                Console.WriteLine($"Ukuran File     : {fileInfo.Length / 1024.0:F2} KB");
                Console.WriteLine($"Output File     : {Path.GetFullPath(outputPath)}");
                Console.WriteLine("--------------------------------");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Pengujian Gagal: {ex.Message}");
            }
        }

        private static List<Transaction> GenerateDummyTransactions(int count)
        {
            var list = new List<Transaction>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(new Transaction
                {
                    Id = i,
                    Date = DateTime.Now.AddMinutes(-i),
                    Amount = i * 100.5,
                    Category = (i % 2 == 0) ? "Income" : "Expense",
                    Description = $"Dummy Transaction Record #{i}"
                });
            }
            return list;
        }
    }
}
