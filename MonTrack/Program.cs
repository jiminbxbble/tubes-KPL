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
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== MonTrack: Performance Testing Feature Ekspor ===");

            // 1. Persiapan 10.000 data dummy
            int recordCount = 10000;
            var dummyData = GenerateDummyTransactions(recordCount);
            string outputPath = "performance_test_export.csv";
            var exportService = new ExportApiService();

            // 2. Mulai Pengukuran Waktu
            Stopwatch stopwatch = new Stopwatch();
            
            Console.WriteLine($"Memulai ekspor {recordCount} data ke {outputPath}...");
            
            stopwatch.Start();
            
            try 
            {
                await exportService.ExecuteExport("CSV", dummyData, outputPath);
                stopwatch.Stop();

                // 3. Menampilkan hasil
                Console.WriteLine("\n--- Hasil Pengujian Performa ---");
                Console.WriteLine($"Jumlah Data     : {recordCount} transaksi");
                Console.WriteLine($"Waktu Eksekusi  : {stopwatch.ElapsedMilliseconds} ms");
                Console.WriteLine($"Status          : Berhasil");
                Console.WriteLine("--------------------------------");
                
                // Cleanup
                if (File.Exists(outputPath)) File.Delete(outputPath);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                Console.WriteLine($"\nPengujian Gagal: {ex.Message}");
            }

            Console.WriteLine("\nTekan tombol apa saja untuk keluar...");
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
